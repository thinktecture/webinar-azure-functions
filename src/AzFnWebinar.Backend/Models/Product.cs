using Newtonsoft.Json;
using System;

namespace AzFnWebinar.Backend.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }
    }
}
