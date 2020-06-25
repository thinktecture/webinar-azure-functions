using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Frontend
{
    public class EasyAuthPrincipal
    {
        [JsonPropertyName("provider_name")]
        public string ProviderName { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("user_claims")]
        public IEnumerable<EasyAuthClaim> Claims { get; set; }
    }
}
