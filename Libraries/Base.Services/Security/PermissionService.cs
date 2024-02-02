using Base.Data;
using Base.Services.Authentication;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Base.Core.Domain.Users;
using Base.Core.Caching;
using System.Threading.Tasks;
using Base.Core.Domain.Security;
using Base.Core;
using Base.Services.Users;

namespace Base.Services.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly IRepository<PermissionRecordRoleMapping> _permissionRecordRoleMappingRepo;
        public PermissionService(IAuthenticationService authenticationService, 
            IWorkContext workContext, IUserService userService, 
            IStaticCacheManager staticCacheManager, IRepository<PermissionRecord> permissionRecordRepository, 
            IRepository<PermissionRecordRoleMapping> permissionRecordRoleMappingRepo)
        {
            this._authenticationService = authenticationService;
            _workContext = workContext;
            _userService = userService;
            _staticCacheManager = staticCacheManager;
            _permissionRecordRepository = permissionRecordRepository;
            _permissionRecordRoleMappingRepo = permissionRecordRoleMappingRepo;
        }

        public virtual async Task<bool> AuthorizeAsync(PermissionRecord permission)
        {
            return await AuthorizeAsync(permission, await _workContext.GetCurrentUserAsync());
        }

        public virtual async Task<bool> AuthorizeAsync(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return await AuthorizeAsync(permission.SystemName, user);
        }
        public virtual async Task<bool> AuthorizeAsync(string permissionRecordSystemName)
        {
            return await AuthorizeAsync(permissionRecordSystemName, await _workContext.GetCurrentUserAsync());
        }

        public virtual async Task<bool> AuthorizeAsync(string permissionRecordSystemName, User user)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = await _userService.GetUserRolesAsync(user);
            foreach (var role in customerRoles)
                if (await AuthorizeAsync(permissionRecordSystemName, role.Id))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        public virtual async Task<bool> AuthorizeAsync(string permissionRecordSystemName, int customerRoleId)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;


            var permissions = await GetPermissionRecordsByUserRoleIdAsync(customerRoleId);
            foreach (var permission in permissions)
                if (permission.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;

        }
        protected virtual async Task<IList<PermissionRecord>> GetPermissionRecordsByUserRoleIdAsync(int userRoleId)
        {
            
            var key = _staticCacheManager.PrepareKeyForDefaultCache(SecurityDefaults.PermissionRecordsAllCacheKey, userRoleId);

            var query = from pr in _permissionRecordRepository.Table
                        join prcrm in _permissionRecordRoleMappingRepo.Table on pr.Id equals prcrm
                            .PermissionRecordId
                        where prcrm.UserRoleId == userRoleId
                        orderby pr.Id
                        select pr;

            return await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());
        }
    }
}
