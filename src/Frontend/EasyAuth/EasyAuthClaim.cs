using System.Text.Json.Serialization;

namespace Frontend
{
    public class EasyAuthClaim
    {
        [JsonPropertyName("typ")]
        public string Type { get; set; }

        [JsonPropertyName("val")]
        public string Value { get; set; }
    }
}
