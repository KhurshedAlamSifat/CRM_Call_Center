using Base.Core.Domain.ComplainTypes;
using FluentMigrator.Builders.Create.Table;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.ComplainTypes
{
    public partial class ComplainTypeBuilder : EntityBuilder<ComplainType>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ComplainType.ComplainTypeName)).AsString(1000).Nullable();
        }
        #endregion
    }
}
