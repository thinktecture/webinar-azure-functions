using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend
{
    public class ProductRepository
    {
        private readonly HttpClient _client;

        public ProductRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<Product[]> GetProducts()
        {
            var response = await _client.GetStringAsync("/api/products");
            return JsonSerializer.Deserialize<Product[]>(response);
        }

        public async Task UpdateProduct(Product product)
        {
            await _client.PutAsJsonAsync($"/api/products/{product.ProductType}/{product.Id}", product);
        }
    }
}
