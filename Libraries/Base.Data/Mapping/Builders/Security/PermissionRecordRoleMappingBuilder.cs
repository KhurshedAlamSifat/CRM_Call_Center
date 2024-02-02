using FluentMigrator.Builders.Create.Table;
using Base.Core.Domain.Users;
using Base.Data.Extensions;
using Base.Core.Domain.Security;

namespace Base.Data.Mapping.Builders.Security
{
    /// <summary>
    /// Represents a customer customer role mapping entity builder
    /// </summary>
    public partial class PermissionRecordRoleMappingBuilder : EntityBuilder<PermissionRecordRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn("PermissionRecordId")
                    .AsInt32().ForeignKey<PermissionRecord>().PrimaryKey()
                .WithColumn("UserRoleId")
                    .AsInt32().ForeignKey<UserRole>().PrimaryKey();
        }

        #endregion
    }
}