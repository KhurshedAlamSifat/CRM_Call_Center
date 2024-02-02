using Base.Core.Domain.Security;
using System;
using System.Collections.Generic;

namespace Base.Core.Domain.Users
{
    public partial class UserRole :BaseEntity
    {
        public string Name { get; set; }
        public bool? Deleted { get; set; }
        public bool IsSystemRole { get; set; }
        public string SystemName { get; set; }

    }
}
