using System.Data;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Users;
using FluentMigrator.Builders.Create.Table;


namespace Base.Data.Mapping.Builders.Distrists
{
    public partial class DistrictBuilder : EntityBuilder<District>
    {

        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(District.DistrictName)).AsString(100).Nullable();
                
        }

        #endregion


    }
}
