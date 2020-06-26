using Newtonsoft.Json;
using System;

namespace AzFnWebinar.Backend.Models
{
    public class Subscription
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public decimal LastPrice { get; set; }
        public string PhoneNumber { get; set; }
        public string ProductId { get; set; }
    }
}
