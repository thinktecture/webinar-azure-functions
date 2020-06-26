using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Security.Claims;

namespace Backend
{
    public static class Subscriptions
    {
        [FunctionName("Subscriptions")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "subscriptions")]
                SubscriptionInputModel subscription,
            ClaimsPrincipal user,
            [CosmosDB("products", "subscriptions", ConnectionStringSetting = "CosmosDBConnection")]
                out Subscription notification
        )
        {
            if (!user.Identity.IsAuthenticated)
            {
                notification = null;
                return new UnauthorizedResult();
            }

            notification = new Subscription()
            {
                Id = Guid.NewGuid(),
                PhoneNumber = subscription.PhoneNumber,
                ProductId = subscription.ProductId
            };

            return new NoContentResult();
        }
    }
}
