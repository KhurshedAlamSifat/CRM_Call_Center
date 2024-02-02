using Base.Core.Domain.Districts;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Technicians;
using Base.Core.Domain.Thanas;
using Base.Data.Extensions;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.ServiceCenters
{
    partial class ServiceCenterBuilder : EntityBuilder<ServiceCenter>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(ServiceCenter.ServiceCenterName)).AsString(50).Nullable()
             .WithColumn("Thana_Id").AsInt32().ForeignKey<Thana>().PrimaryKey();

        }

    }
    
}
