using System;
using System.Collections.Generic;
using System.Text;
using Base.Data;
using Base.Services.Security;
using System.Collections.Immutable;
using Base.Core.Domain.Users;
using System.Threading.Tasks;
using Base.Core.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Base.Core;
using DocumentFormat.OpenXml.Bibliography;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Base.Services.Authentication;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Base.Services.Users
{
    public class UserService : IUserService
    {

        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<User> _userRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IRepository<UserPassword> _userPasswordRepo;
        private readonly IRepository<UserRole> _userRoleRepo;
        private readonly IRepository<UserUserRoleMapping> _userUserRoleRepo;

        public UserService(IEncryptionService encryptionService, IConfiguration configuration, IWebHostEnvironment environment,
            IRepository<User> userRepository, IRepository<UserPassword> userPasswordRepo,
            IStaticCacheManager staticCacheManager, IRepository<UserRole> userRoleRepo, IRepository<UserUserRoleMapping> userUserRoleRepo)
        {
            this._encryptionService = encryptionService;
            _configuration = configuration;
            _environment = environment;
            _userRepository = userRepository;
            _staticCacheManager = staticCacheManager;
            _userPasswordRepo = userPasswordRepo;
            _userRoleRepo = userRoleRepo;
            _userUserRoleRepo = userUserRoleRepo;
        }



        #region Usuarios

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id == null)
                return null;
            var user = await _userRepository.Table.FirstOrDefaultAsync(p => p.Id == id);

            return user;

        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var cacheKey = new CacheKey($"Crm.User.byemail.{email}", "Crm.User.byemail.");
            var cacheElement = _staticCacheManager.PrepareKey(cacheKey, email);

            var user = await _staticCacheManager.GetAsync(cacheElement, async () =>
            {
                return await _userRepository.Table.FirstOrDefaultAsync(p => p.Email == email) ?? new User();
            });
            return user;
        }

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            var cacheKey = new CacheKey($"Crm.User.byphone.{phone}", "Crm.User.byphone.");
            var cacheElement = _staticCacheManager.PrepareKey(cacheKey, phone);

            var user = await _staticCacheManager.GetAsync(cacheElement, async () =>
            {
                return await _userRepository.Table.FirstOrDefaultAsync(p => p.Phone == phone) ?? new User();
            });
            return user;
        }

        public async Task<List<User>> GetTopTenActiveUserAsync()
        {
            var users = await _userRepository.Table.OrderByDescending(y => y.Id).Take(10).ToListAsync();
            return users;
        }

        public async Task HardDeleteUser(User user)
        {
            await _userRepository.DeleteAsync(user);
        }

        public virtual async Task<bool> IsCallCenterAgentAsync(User user)
        {
            return await IsInUserRoleAsync(user, CrmUserDefaults.CallCentreAgentRoleName);
        }

        public virtual async Task<bool> IsAdminAsync(User user)
        {
            return await IsInUserRoleAsync(user, CrmUserDefaults.AdministratorsRoleName);
        }

        public virtual async Task<bool> IsInUserRoleAsync(User user,
            string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException(nameof(customerRoleSystemName));

            var customerRoles = await GetUserRolesAsync(user);

            return customerRoles?.Any(cr => cr.SystemName == customerRoleSystemName) ?? false;

        }
        public virtual async Task<IList<UserRole>> GetUserRolesAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));


            return await _userRoleRepo.GetAllAsync(query =>
            {
                return from cr in query
                       join crm in _userUserRoleRepo.Table on cr.Id equals crm.UserRoleId
                       where crm.UserId == user.Id
                       select cr;
            }, cache => cache.PrepareKeyForShortTermCache(CrmUserServicesDefaults.UserRolesCacheKey, user, true));

        }
        public virtual async Task<User> GetUserByGuidAsync(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return null;

            var query = from c in _userRepository.Table
                        where c.UserGuid == userGuid
                        orderby c.Id
                        select c;


            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }


        public virtual async Task InsertUserAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        public virtual async Task InsertUserPasswordAsync(UserPassword userPassword)
        {
            await _userPasswordRepo.InsertAsync(userPassword);
        }


        public virtual async Task<UserLoginResults> ValidateUserAsync(string phoneNo, string password)
        {
            var user = await GetUserByPhoneAsync(phoneNo);

            if (user == null)
                return UserLoginResults.UserNotExist;

            //only registered can login
            if (!await IsCallCenterAgentAsync(user))
            {
                if (!await IsAdminAsync(user))
                    return UserLoginResults.NotRegistered;
            }

            if (!PasswordsMatch(await GetUserPasswordAsync(user.Id), password))
            {
                return UserLoginResults.WrongPassword;
            }
            return UserLoginResults.Successful;
        }
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, userPassword.PasswordSalt, "SHA1");
                    break;
            }

            if (userPassword.Password == null)
                return false;

            return userPassword.Password.Equals(savedPassword);
        }

        public virtual async Task<UserPassword> GetUserPasswordAsync(int userId)
        {
            if (userId == 0)
                return null;

            //return the latest password
            return (await GetUserPasswordsAsync(userId, passwordsToReturn: 1)).FirstOrDefault();
        }

        public virtual async Task<IList<UserPassword>> GetUserPasswordsAsync(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _userPasswordRepo.Table.AsQueryable();

            //filter by customer
            if (userId.HasValue)
                query = query.Where(password => password.UserId == userId.Value);

            //filter by password format
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)passwordFormat.Value);

            //get the latest passwords
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOnUtc).Take(passwordsToReturn.Value);

            return await query.ToListAsync();
        }

        public virtual async Task<UserRole> GetUserRoleBySystemNameAsync(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;


            var cacheKey = new CacheKey($"Crm.User.bysystemname.{systemName}", "Crm.User.bysystemname.");
            var cacheElement = _staticCacheManager.PrepareKey(cacheKey, systemName);

            var userRole = await _staticCacheManager.GetAsync(cacheElement, async () =>
            {

                var query = from cr in _userRoleRepo.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;

                return await query.FirstOrDefaultAsync();
            });

            return userRole;
        }

        public async Task AddUserRoleMappingAsync(UserUserRoleMapping roleMapping)
        {
            await _userUserRoleRepo.InsertAsync(roleMapping);
        }

        public virtual async Task<IPagedList<User>> GetAllUsersAsync(string phone = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var users = await _userRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(phone))
                    query = query.Where(c => c.Phone == phone);

                query = query.OrderByDescending(c => c.Id);

                return query;
            }, pageIndex, pageSize, getOnlyTotalCount);

            return users;
        }

        #endregion

    }
}
