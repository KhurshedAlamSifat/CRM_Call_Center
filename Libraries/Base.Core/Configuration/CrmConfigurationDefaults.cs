namespace Base.Core.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class CrmConfigurationDefaults
    {
        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppSettingsFilePath => "App_Data/appsettings.json";

        public static string AppSettingsEnvironmentFilePath => "App_Data/appsettings.{0}.json";
    }
}