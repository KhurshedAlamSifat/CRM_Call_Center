using System.Collections.Generic;
using Base.Core.Domain.Users;
using Base.Core.Domain.Security;

namespace Base.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessSecurePanel = new PermissionRecord { Name = "Access secure area", SystemName = "AccessSecurePanel", Category = "Standard" };
        
        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessSecurePanel,
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    "Administrators",
                    new[]
                    {
                        AccessSecurePanel,
                    }
                )
            };
        }
    }
}