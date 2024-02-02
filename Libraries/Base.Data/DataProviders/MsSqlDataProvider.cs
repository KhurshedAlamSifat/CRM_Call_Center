using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Data.SqlClient;
using Base.Core;

namespace Base.Data.DataProviders
{
    /// <summary>
    /// Represents the MS SQL Server data provider
    /// </summary>
    public partial class MsSqlNopDataProvider : BaseDataProvider, ICRMDataProvider
    {
        #region Utils

        protected virtual SqlConnectionStringBuilder GetConnectionStringBuilder()
        {
            var connectionString = DataSettingsManager.LoadSettings().ConnectionString;

            return new SqlConnectionStringBuilder(connectionString);
        }

        #endregion

        #region Utils

        /// <summary>
        /// Gets a connection to the database for a current data provider
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Connection to a database</returns>
        protected override DbConnection GetInternalDbConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            return new SqlConnection(connectionString);
        }

        #endregion

        #region Methods


        /// <summary>
        /// Get the current identity value
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the integer identity; null if cannot get the result
        /// </returns>
        public virtual Task<int?> GetTableIdentAsync<TEntity>() where TEntity : BaseEntity
        {
            using var currentConnection = CreateDataConnection();
            var tableName = GetEntityDescriptor(typeof(TEntity)).EntityName;

            var result = currentConnection.Query<decimal?>($"SELECT IDENT_CURRENT('[{tableName}]') as Value")
                .FirstOrDefault();

            return Task.FromResult<int?>(result.HasValue ? Convert.ToInt32(result) : 1);
        }

        /// <summary>
        /// Set table identity (is supported)
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="ident">Identity value</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SetTableIdentAsync<TEntity>(int ident) where TEntity : BaseEntity
        {
            using var currentConnection = CreateDataConnection();
            var currentIdent = await GetTableIdentAsync<TEntity>();
            if (!currentIdent.HasValue || ident <= currentIdent.Value)
                return;

            var tableName = GetEntityDescriptor(typeof(TEntity)).EntityName;

            await currentConnection.ExecuteAsync($"DBCC CHECKIDENT([{tableName}], RESEED, {ident})");
        }


        /// <summary>
        /// Re-index database tables
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task ReIndexTablesAsync()
        {
            using var currentConnection = CreateDataConnection();
            var commandText = $@"
                    DECLARE @TableName sysname 
                    DECLARE cur_reindex CURSOR FOR
                    SELECT table_name
                    FROM [{currentConnection.Connection.Database}].information_schema.tables
                    WHERE table_type = 'base table'
                    OPEN cur_reindex
                    FETCH NEXT FROM cur_reindex INTO @TableName
                    WHILE @@FETCH_STATUS = 0
                        BEGIN
                            exec('ALTER INDEX ALL ON [' + @TableName + '] REBUILD')
                            FETCH NEXT FROM cur_reindex INTO @TableName
                        END
                    CLOSE cur_reindex
                    DEALLOCATE cur_reindex";

            await currentConnection.ExecuteAsync(commandText);
        }

        public virtual string BuildConnectionString(ICRMConnectionStringInfo nopConnectionString)
        {
            if (nopConnectionString is null)
                throw new ArgumentNullException(nameof(nopConnectionString));

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = nopConnectionString.ServerName,
                InitialCatalog = nopConnectionString.DatabaseName,
                PersistSecurityInfo = false,
                IntegratedSecurity = nopConnectionString.IntegratedSecurity,
                TrustServerCertificate = true
            };

            if (!nopConnectionString.IntegratedSecurity)
            {
                builder.UserID = nopConnectionString.Username;
                builder.Password = nopConnectionString.Password;
            }

            return builder.ConnectionString;
        }

        /// <summary>
        /// Gets the name of a foreign key
        /// </summary>
        /// <param name="foreignTable">Foreign key table</param>
        /// <param name="foreignColumn">Foreign key column name</param>
        /// <param name="primaryTable">Primary table</param>
        /// <param name="primaryColumn">Primary key column name</param>
        /// <returns>Name of a foreign key</returns>
        public virtual string CreateForeignKeyName(string foreignTable, string foreignColumn, string primaryTable, string primaryColumn)
        {
            return $"FK_{foreignTable}_{foreignColumn}_{primaryTable}_{primaryColumn}";
        }

        /// <summary>
        /// Gets the name of an index
        /// </summary>
        /// <param name="targetTable">Target table name</param>
        /// <param name="targetColumn">Target column name</param>
        /// <returns>Name of an index</returns>
        public virtual string GetIndexName(string targetTable, string targetColumn)
        {
            return $"IX_{targetTable}_{targetColumn}";
        }

        /// <summary>
        /// Updates records in table, using values from entity parameter.
        /// Records to update are identified by match on primary key value from obj value.
        /// </summary>
        /// <param name="entities">Entities with data to update</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            using var dataContext = CreateDataConnection();
            await dataContext.GetTable<TEntity>()
                .Merge()
                .Using(entities)
                .OnTargetKey()
                .UpdateWhenMatched()
                .MergeAsync();
        }

        /// <summary>
        /// Updates records in table, using values from entity parameter.
        /// Records to update are identified by match on primary key value from obj value.
        /// </summary>
        /// <param name="entities">Entities with data to update</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        public override void UpdateEntities<TEntity>(IEnumerable<TEntity> entities)
        {
            using var dataContext = CreateDataConnection();
            dataContext.GetTable<TEntity>()
                .Merge()
                .Using(entities)
                .OnTargetKey()
                .UpdateWhenMatched()
                .Merge();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sql server data provider
        /// </summary>
        protected override IDataProvider LinqToDbDataProvider => SqlServerTools.GetDataProvider(SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient);

        /// <summary>
        /// Gets allowed a limit input value of the data for hashing functions, returns 0 if not limited
        /// </summary>
        public int SupportedLengthOfBinaryHash { get; } = 8000;

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup
        /// </summary>
        public virtual bool BackupSupported => true;

        #endregion
    }
}
