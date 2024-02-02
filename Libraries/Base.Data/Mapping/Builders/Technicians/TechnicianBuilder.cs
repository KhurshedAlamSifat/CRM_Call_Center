using Base.Core.Domain.Categories;
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

namespace Base.Data.Mapping.Builders.Technicians
{
    public partial class TechnicianBuilder : EntityBuilder<Technician>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Technician.TechnicianName)).AsString(50).NotNullable()
             .WithColumn(nameof(Technician.TechnicianPhoneNo)).AsString(50).NotNullable()
             .WithColumn("Thana_Id").AsInt32().ForeignKey<Thana>().PrimaryKey().Nullable()
             .WithColumn("ServiceCenter_Id").AsInt32().ForeignKey<ServiceCenter>().PrimaryKey().Nullable()
             .WithColumn("Category_Id").AsInt32().ForeignKey<Category>().PrimaryKey().Nullable();

        }
    }
}
