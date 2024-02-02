using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Customers;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Thanas;
using FluentMigrator.Builders.Create.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Mapping.Builders.Customers
{
    public class CustomerBuilder : EntityBuilder<Customer>
    {
        #region Methods
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Customer.PhoneNumber)).AsString().NotNullable().Unique()
                .WithColumn(nameof(Customer.District_Id)).AsInt16().NotNullable().ForeignKey(nameof(District), nameof(Customer.District_Id))
                .WithColumn(nameof(Customer.Thana_Id)).AsInt16().NotNullable().ForeignKey(nameof(Thana), nameof(Customer.Thana_Id));

        }

        #endregion
    }
}
