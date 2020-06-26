using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Frontend
{
    public class SubscriptionService
    {
        private readonly HttpClient _client;

        public SubscriptionService(HttpClient client) =>
            _client = client;

        public async Task Subscribe(Subscribtion subscribtion) =>
            await _client.PostAsJsonAsync("/api/subscriptions", subscribtion);
    }
}
