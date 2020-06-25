using Newtonsoft.Json;
using System;

namespace AzFnWebinar.Shared
{
    public class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }
    }

    public class ProductInputModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
    }

}
