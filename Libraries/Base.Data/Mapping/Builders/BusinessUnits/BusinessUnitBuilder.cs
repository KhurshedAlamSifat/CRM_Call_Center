using Base.Core.Domain.BusinessUnits;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.BusinessUnits
{
    public class BusinessUnitBuilder : EntityBuilder<BusinessUnit>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(BusinessUnit.BusinessUnitName)).AsString(150).Nullable();
        }
        #endregion
    }
}
