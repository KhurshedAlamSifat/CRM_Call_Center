using FluentMigrator.Builders.Create.Table;
using Base.Core.Domain.Users;

namespace Base.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a customer role entity builder
    /// </summary>
    public partial class UserRoleBuilder : EntityBuilder<UserRole>
    {
        #region Methods
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(UserRole.Name)).AsString(255).NotNullable()
                .WithColumn(nameof(UserRole.SystemName)).AsString(255).Nullable();
        }

        #endregion
    }
}