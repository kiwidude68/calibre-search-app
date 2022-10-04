using System.Text.Json.Serialization;

namespace CalibreSearchApp.Tester
{
    internal class Manifest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("allowed_extensions")]
        public string[] AllowedExtensions { get; set; }

        [JsonPropertyName("allowed_origins")]
        public string[] AllowedOrigins { get; set; }
    }
}
