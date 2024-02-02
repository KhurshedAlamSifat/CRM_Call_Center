using System.Data;
using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Users;
using FluentMigrator.Builders.Create.Table;


namespace Base.Data.Mapping.Builders.Brands
{
    /// <summary>
    /// Represents a customer entity builder
    /// </summary>
    public partial class BrandBuilder : EntityBuilder<Brand>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Brand.BrandName)).AsString(150).NotNullable()
                .WithColumn(nameof(Brand.BusinessUnit_Id)).AsInt16().NotNullable().ForeignKey(nameof(BusinessUnit), nameof(Brand.BusinessUnit_Id));

        }

        #endregion
    }
}
