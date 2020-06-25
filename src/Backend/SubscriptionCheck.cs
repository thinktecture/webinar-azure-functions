// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using AzFnWebinar.Shared;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public static class SubscriptionCheck
    {
        [FunctionName("SubscriptionCheck")]
        public static async Task Run(
            [EventGridTrigger] EventGridEvent eventGridEvent,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            [Queue("sms-queue", Connection = "WebinarStorage")] IAsyncCollector<Message> messages, ILogger log)
        {
            var obj = (JObject)eventGridEvent.Data;
            var product = obj.ToObject<Product>();
            if (product == null)
            {
                log.LogError("Event has no product. Actual data {data}", eventGridEvent.Data);
                return;
            }

            var collectionUri = UriFactory.CreateDocumentCollectionUri("products", "subscriptions");
            var query = client.CreateDocumentQuery<NotificationSubscription>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(s => s.ProductId == product.ProductId)
                .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                foreach (NotificationSubscription subscription in await query.ExecuteNextAsync())
                {
                    await messages.AddAsync(new Message()
                    {
                        PhoneNumber = subscription.PhonenNumber,
                        Body = $"Das Produkt {product.Name} ist wieder verfuegbar!"
                    });
                }
            }
        }
    }
}
