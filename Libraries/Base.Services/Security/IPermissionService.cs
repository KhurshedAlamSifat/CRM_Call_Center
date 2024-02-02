using Base.Core.Domain.Security;
using Base.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Security
{
    public interface IPermissionService
    {
        Task<bool> AuthorizeAsync(PermissionRecord permission);

        Task<bool> AuthorizeAsync(PermissionRecord permission, User user);
        Task<bool> AuthorizeAsync(string permissionRecordSystemName);
        Task<bool> AuthorizeAsync(string permissionRecordSystemName, User user);

        Task<bool> AuthorizeAsync(string permissionRecordSystemName, int userRoleId);

    }

}
