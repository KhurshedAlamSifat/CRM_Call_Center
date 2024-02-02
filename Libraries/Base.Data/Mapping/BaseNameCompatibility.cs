using System;
using System.Collections.Generic;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Security;
using Base.Core.Domain.Thanas;
using Base.Core.Domain.Users;


namespace Base.Data.Mapping
{
    /// <summary>
    /// Base instance of backward compatibility of table naming
    /// </summary>
    public partial class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new()
        {
            { typeof(District), "Districts" },
            {  typeof(Thana), "Thana" },
            { typeof(UserUserRoleMapping), "User_UserRole_Mapping" },
            { typeof(PermissionRecordRoleMapping), "PermissionRecord_Role_Mapping" }

        };

        public Dictionary<(Type, string), string> ColumnName => new()
        {
            { (typeof(UserUserRoleMapping), "UserId"), "User_Id" },
            { (typeof(UserUserRoleMapping), "UserRoleId"), "UserRole_Id" }

        };
    }
}