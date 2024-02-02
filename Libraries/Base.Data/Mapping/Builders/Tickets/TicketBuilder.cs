using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Categories;
using Base.Core.Domain.ComplainTypes;
using Base.Core.Domain.Customers;
using Base.Core.Domain.Districts;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Technicians;
using Base.Core.Domain.Thanas;
using Base.Core.Domain.Tickets;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.Tickets
{
    public class TicketBuilder : EntityBuilder<Ticket>
    {
        #region Methods
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Ticket.CustomerPhoneNo)).AsString(50).NotNullable()
                .WithColumn(nameof(Ticket.ProblemIds)).AsString(50).NotNullable()
                .WithColumn(nameof(Ticket.Note)).AsString(100).NotNullable()
                .WithColumn(nameof(Ticket.ComplainType_Id)).AsInt16().NotNullable().ForeignKey(nameof(ComplainType), nameof(Ticket.ComplainType_Id))
                .WithColumn(nameof(Ticket.Customer_Id)).AsInt16().NotNullable().ForeignKey(nameof(Customer), nameof(Ticket.Customer_Id))
                .WithColumn(nameof(Ticket.Technician_Id)).AsInt16().Nullable().ForeignKey(nameof(Technician), nameof(Ticket.Technician_Id))
                .WithColumn(nameof(Ticket.District_Id)).AsInt16().NotNullable().ForeignKey(nameof(District), nameof(Ticket.District_Id))
                .WithColumn(nameof(Ticket.Thana_Id)).AsInt16().NotNullable().ForeignKey(nameof(Thana), nameof(Ticket.Thana_Id))
                .WithColumn(nameof(Ticket.Business_Id)).AsInt16().NotNullable().ForeignKey(nameof(BusinessUnit), nameof(Ticket.Business_Id))
                .WithColumn(nameof(Ticket.Brand_Id)).AsInt16().NotNullable().ForeignKey(nameof(Brand), nameof(Ticket.Brand_Id))
                .WithColumn(nameof(Ticket.Category_Id)).AsInt16().NotNullable().ForeignKey(nameof(Category), nameof(Ticket.Category_Id))
                .WithColumn(nameof(Ticket.ServiceCenter_Id)).AsInt16().Nullable().ForeignKey(nameof(ServiceCenter), nameof(Ticket.ServiceCenter_Id));
        }

        #endregion
    }
}
