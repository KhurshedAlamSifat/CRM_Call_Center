using FluentMigrator.Builders.Create.Table;
using Base.Core.Domain.Users;
using Base.Data.Extensions;

namespace Base.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a customer customer role mapping entity builder
    /// </summary>
    public partial class UserUserRoleMappingBuilder : EntityBuilder<UserUserRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn("User_Id")
                    .AsInt32().ForeignKey<User>().PrimaryKey()
                .WithColumn("UserRole_Id")
                    .AsInt32().ForeignKey<UserRole>().PrimaryKey();
        }

        #endregion
    }
}