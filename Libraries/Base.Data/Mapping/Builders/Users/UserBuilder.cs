using System.Data;
using Base.Core.Domain.Users;
using FluentMigrator.Builders.Create.Table;


namespace Base.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a customer entity builder
    /// </summary>
    public partial class UserBuilder : EntityBuilder<User>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(User.Username)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Email)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Name)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Gender)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Company)).AsString(1000).Nullable()
                .WithColumn(nameof(User.HomeAddress)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Phone)).AsString(1000).Nullable();
        }

        #endregion
    }
}