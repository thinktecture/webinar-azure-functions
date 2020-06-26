using System.Text.Json.Serialization;

namespace AzFnWebinar.Frontend.Authentication
{
    public class EasyAuthClaim
    {
        [JsonPropertyName("typ")]
        public string Type { get; set; }

        [JsonPropertyName("val")]
        public string Value { get; set; }
    }
}
