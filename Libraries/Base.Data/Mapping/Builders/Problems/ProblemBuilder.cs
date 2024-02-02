using Base.Core.Domain.Brands;
using Base.Core.Domain.Categories;
using Base.Core.Domain.ComplainTypes;
using Base.Core.Domain.Problems;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.Problems
{
    internal class ProblemBuilder : EntityBuilder<Problem>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Problem.ProblemDescription)).AsString(1000).NotNullable()
                 .WithColumn(nameof(Problem.CategoryId)).AsInt16().NotNullable().ForeignKey(nameof(Category), nameof(Problem.CategoryId));
        }
        #endregion
    }
}
