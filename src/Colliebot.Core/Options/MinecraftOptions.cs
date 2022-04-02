using Tomlyn.Model;

namespace Colliebot
{
    public class MinecraftOptions : ITomlMetadataProvider
    {
        public TomlPropertiesMetadata? PropertiesMetadata { get; set; }

        public string ServerAddress = "localhost";
        public string ServerUsername = "root";
        public string ServerPassword = "";
    }
}
