using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder
                .Services
                .AddAuthorizationCore(o =>
                {
                    o.AddPolicy("is-admin", builder => builder.RequireRole("ProductAdmin"));
                });
            builder.Services.AddScoped<AuthenticationStateProvider, EasyAuthAuthenticationStateProvider>();
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }
    }
}
