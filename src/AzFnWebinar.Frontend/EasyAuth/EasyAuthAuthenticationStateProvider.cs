using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzFnWebinar.Frontend.Authentication
{
    public class EasyAuthAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _client;

        public EasyAuthAuthenticationStateProvider(HttpClient client) : base()
        {
            _client = client;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var response = await _client.GetAsync("/.auth/me");
            if (!response.IsSuccessStatusCode)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            EasyAuthPrincipal[] principals;
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                principals = JsonSerializer.Deserialize<EasyAuthPrincipal[]>(content);
            }
            catch (JsonException)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var principal = principals.FirstOrDefault();
            if (principal == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(principal.ProviderName, "name", "roles");
            identity.AddClaims(principal.Claims.Select(c => new Claim(c.Type, c.Value)));
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
