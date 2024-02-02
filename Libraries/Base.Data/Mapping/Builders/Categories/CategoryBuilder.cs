using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Core.Domain.Categories;
using Base.Core.Domain.BusinessUnits;
using FluentMigrator.Builders.Create.Table;
using Base.Core.Domain.Brands;

namespace Base.Data.Mapping.Builders.Categories
{
    public partial class CategoryBuilder : EntityBuilder<Category>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Category.CategoryName)).AsString(150).NotNullable()
                .WithColumn(nameof(Category.Brand_Id)).AsInt16().NotNullable().ForeignKey(nameof(Brand), nameof(Category.Brand_Id));

        }

        #endregion
    }
}
