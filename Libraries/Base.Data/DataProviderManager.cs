using Base.Core;
using Base.Core.Infrastructure;
using Base.Data.Configuration;
using Base.Data.DataProviders;
using System;

namespace Base.Data
{
    /// <summary>
    /// Represents the data provider manager
    /// </summary>
    public partial class DataProviderManager : IDataProviderManager
    {
        #region Methods

        /// <summary>
        /// Gets data provider by specific type
        /// </summary>
        /// <param name="dataProviderType">Data provider type</param>
        /// <returns></returns>
        public static ICRMDataProvider GetDataProvider(DataProviderType dataProviderType)
        {
            return dataProviderType switch
            {
                DataProviderType.SqlServer => new MsSqlNopDataProvider(),
                _ => throw new Exception($"Not supported data provider name: '{dataProviderType}'"),
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public ICRMDataProvider DataProvider
        {
            get
            {
                var dataProviderType = DataProviderType.SqlServer;

                return GetDataProvider(dataProviderType);
            }
        }

        #endregion
    }
}
