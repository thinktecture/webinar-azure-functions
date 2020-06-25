using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend
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
            var response = await _client.GetStringAsync("/.auth/me");
            EasyAuthPrincipal[] principals;
            try
            {
                principals = JsonSerializer.Deserialize<EasyAuthPrincipal[]>(response);
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
