using Tomlyn.Model;

namespace Colliebot
{
    public class LoggingOptions : ITomlMetadataProvider
    {
        public TomlPropertiesMetadata? PropertiesMetadata { get; set; }

        public string OutputPath { get; } = "./common/logs";
        public int MaxFileSizeKb { get; } = 500;
    }
}
