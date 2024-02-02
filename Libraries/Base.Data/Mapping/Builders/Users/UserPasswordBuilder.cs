using System.Data;
using Base.Core.Domain.Users;
using FluentMigrator.Builders.Create.Table;


namespace Base.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a customer entity builder
    /// </summary>
    public partial class UserPasswordBuilder : EntityBuilder<UserPassword>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(UserPassword.Password)).AsString(1000).NotNullable()
                .WithColumn(nameof(UserPassword.UserId)).AsInt16().NotNullable().ForeignKey(nameof(User),nameof(UserPassword.UserId));

        }

        #endregion
    }
}