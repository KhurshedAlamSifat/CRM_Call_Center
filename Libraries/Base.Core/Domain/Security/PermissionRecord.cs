using Base.Core.Domain.Users;
using System.Collections.Generic;

namespace Base.Core.Domain.Security
{
    public partial class PermissionRecord : BaseEntity
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public string Category { get; set; }
    }
}
