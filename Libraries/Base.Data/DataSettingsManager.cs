using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Base.Core;
using Base.Core.Configuration;
using Base.Core.Infrastructure;
using Base.Data.Configuration;
using Newtonsoft.Json;


namespace Base.Data
{
    /// <summary>
    /// Represents the data settings manager
    /// </summary>
    public partial class DataSettingsManager
    {
        #region Fields
        #endregion
        /// <summary>
        /// Gets data settings from the old json file (dataSettings.json)
        /// </summary>
        /// <param name="data">Old json file data</param>
        /// <returns>Data settings</returns>
        protected static DataConfig LoadDataSettingsFromOldJsonFile(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var jsonDataSettings = JsonConvert.DeserializeAnonymousType(data,
                new { DataConnectionString = "", DataProvider = DataProviderType.SqlServer, SQLCommandTimeout = "" });
            var dataSettings = new DataConfig
            {
                ConnectionString = jsonDataSettings.DataConnectionString,
                DataProvider = jsonDataSettings.DataProvider,
                SQLCommandTimeout = int.TryParse(jsonDataSettings.SQLCommandTimeout, out var result) ? result : null
            };

            return dataSettings;
        }


        #region Methods

        /// <summary>
        /// Load data settings
        /// </summary>
        /// <param name="fileProvider">File provider</param>
        /// <param name="reload">Force loading settings from disk</param>
        /// <returns>Data settings</returns>
        public static DataConfig LoadSettings(ICrmFileProvider fileProvider = null, bool reload = false)
        {
            if (!reload && Singleton<DataConfig>.Instance is not null)
                return Singleton<DataConfig>.Instance;

            //backward compatibility
            fileProvider ??= CommonHelper.DefaultFileProvider;
            var filePath_json = fileProvider.MapPath(CrmDataSettingsDefaults.FilePath);
            if (fileProvider.FileExists(filePath_json))
            {
                var dataSettings = fileProvider.FileExists(filePath_json)
                    ? LoadDataSettingsFromOldJsonFile(fileProvider.ReadAllText(filePath_json, Encoding.UTF8))
                    : new DataConfig();

                fileProvider.DeleteFile(filePath_json);
                Singleton<DataConfig>.Instance = dataSettings;
            }
            else
            {
                Singleton<DataConfig>.Instance = Singleton<AppSettings>.Instance.Get<DataConfig>();
            }

            return Singleton<DataConfig>.Instance;
        }



        /// <summary>
        /// Gets the command execution timeout.
        /// </summary>
        /// <value>
        /// Number of seconds. Negative timeout value means that a default timeout will be used. 0 timeout value corresponds to infinite timeout.
        /// </value>
        public static int GetSqlCommandTimeout()
        {
            return LoadSettings()?.SQLCommandTimeout ?? -1;
        }

        #endregion
    }
}