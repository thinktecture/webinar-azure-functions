using System;
using System.Text.Json.Serialization;

namespace Frontend
{
    public class Product
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("productType")]
        public string ProductType { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class Subscribtion
    {
        public string ProductId { get; set; }

        public string Phonenumber { get; set; }
    }
}
