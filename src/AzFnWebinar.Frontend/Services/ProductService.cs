using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AzFnWebinar.Frontend.Services
{
    public class ProductService
    {
        private readonly HttpClient _client;

        public ProductService(HttpClient client) =>
            _client = client;

        public async Task<Product[]> GetProducts() =>
            await _client.GetFromJsonAsync<Product[]>("/api/products");

        public async Task UpdateProduct(Product product) =>
            await _client.PutAsJsonAsync($"/api/products/{product.ProductType}/{product.Id}", product);
    }
}
