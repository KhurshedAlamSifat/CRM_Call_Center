using System.Runtime.Serialization;

namespace Base.Data
{
    /// <summary>
    /// Represents data provider type enumeration
    /// </summary>
    public enum DataProviderType
    {
        [EnumMember(Value = "Unknown")]
        Unknown,
        [EnumMember(Value = "sqlserver")]
        SqlServer,
    }
}