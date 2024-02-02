using Base.Core.Domain.Users;

namespace Base.Core.Domain.Security
{
    /// <summary>
    /// Represents a permission record-customer role mapping class
    /// </summary>
    public partial class PermissionRecordRoleMapping : BaseEntity
    {
        public int PermissionRecordId { get; set; }

        public int UserRoleId { get; set; }
    }
}