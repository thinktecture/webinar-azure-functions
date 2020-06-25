// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Newtonsoft.Json;
using System;

namespace Backend
{

    public class PriceNotification
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string PhonenNumber { get; set; }
        public decimal LastPrice { get; set; }
    }
}
