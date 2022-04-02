using Tomlyn.Model;

namespace Colliebot
{
    public class AppOptions : ITomlMetadataProvider
    {
        public TomlPropertiesMetadata? PropertiesMetadata { get; set; }

        public bool IsDebugEnabled = false;

        public object Discord { get; set; } = new();
        public object Twitch { get; set; } = new();
        public object Commands { get; set; } = new();
        public object Scripting { get; set; } = new();
        public LoggingOptions Logging { get; set; } = new();

    }
}
