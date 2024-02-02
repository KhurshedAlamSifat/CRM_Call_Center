using Base.Core;
using Base.Core.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<UserRole> GetUserRoleBySystemNameAsync(string systemName); 
        Task<List<User>> GetTopTenActiveUserAsync();
        Task HardDeleteUser(User user);
        Task<bool> IsCallCenterAgentAsync(User user);
        Task<bool> IsAdminAsync(User user);

        Task<bool> IsInUserRoleAsync(User user,
        string customerRoleSystemName, bool onlyActiveCustomerRoles = true);
        Task<IList<UserRole>> GetUserRolesAsync(User user);
        Task<User> GetUserByGuidAsync(Guid userGuid);
        Task UpdateUserAsync(User user);
        Task InsertUserAsync(User user);
        Task InsertUserPasswordAsync(UserPassword userPassword); 
        Task<UserLoginResults> ValidateUserAsync(string phoneNo, string password);
        Task<User> GetUserByPhoneAsync(string phone);
        Task AddUserRoleMappingAsync(UserUserRoleMapping roleMapping);
        Task<IPagedList<User>> GetAllUsersAsync(string phone = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
    }
}
