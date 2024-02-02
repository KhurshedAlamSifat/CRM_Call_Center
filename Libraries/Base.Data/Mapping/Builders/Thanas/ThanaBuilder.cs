using Base.Core.Domain.Districts;
using Base.Core.Domain.Thanas;
using Base.Core.Domain.Users;
using Base.Data.Extensions;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.Thanas
{
    public partial class ThanaBuilder : EntityBuilder<Thana>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        /// 
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Thana.ThanaName)).AsString(100).Nullable()
            
                .WithColumn("District_Id").AsInt32().ForeignKey<District>().PrimaryKey();

        }
        #endregion
    }
}
