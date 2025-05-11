using System.Text.Json.Serialization;

namespace DestinationTravelAPP.Models
{
    public class Country
    {
        [JsonPropertyName("name")]
        public NameInfo? Name { get; set; }

        [JsonPropertyName("cca2")]
        public string? Code { get; set; }

        [JsonPropertyName("flag")]
        public string? Flag { get; set; }

        public class NameInfo
        {
            [JsonPropertyName("common")]
            public string? Common { get; set; }
        }
    }
}
