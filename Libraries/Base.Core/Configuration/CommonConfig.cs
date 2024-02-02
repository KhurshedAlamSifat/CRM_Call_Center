namespace Base.Core.Configuration
{
    /// <summary>
    /// Represents common configuration parameters
    /// </summary>
    public partial class CommonConfig : IConfig
    {
        public bool DisplayFullErrorStack { get; private set; } = false;
        public bool UseSessionStateTempDataProvider { get; private set; } = false;
        public bool MiniProfilerEnabled { get; private set; } = false;
        public string StaticFilesCacheControl { get; private set; } = "public,max-age=31536000";
        public bool ServeUnknownFileTypes { get; private set; } = false;
        public bool UseAutofac { get; set; } = true;
    }
}