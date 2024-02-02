using System.Configuration;
using FluentMigrator.Runner.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Base.Core.Configuration;

namespace Base.Data.Configuration
{
    public partial class DataConfig : IConfig, IConnectionStringAccessor
    {
        public string ConnectionString { get; set; } = string.Empty;

        [JsonConverter(typeof(StringEnumConverter))]
        public DataProviderType DataProvider { get; set; } = DataProviderType.SqlServer;
        public int? SQLCommandTimeout { get; set; } = null;
        [JsonIgnore]
        public string Name => nameof(ConfigurationManager.ConnectionStrings);
        public int GetOrder() => 0; //display first
    }
}