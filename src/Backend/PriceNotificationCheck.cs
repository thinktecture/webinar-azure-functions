// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using AzFnWebinar.Shared;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public static class PriceNotificationCheck
    {
        [FunctionName("PriceNotificationCheck")]
        public static async Task Run(
            [EventGridTrigger]
                EventGridEvent eventGridEvent,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            [Queue("sms-queue", Connection = "WebinarStorage")]
                IAsyncCollector<Message> messages,
            [CosmosDB("products", "subscriptions", ConnectionStringSetting = "CosmosDBConnection")]
                IAsyncCollector<Subscription> subscriptions)
        {
            var obj = (JObject)eventGridEvent.Data;
            var product = obj.ToObject<Product>();
            if (product == null)
            {
                return;
            }

            var collectionUri = UriFactory.CreateDocumentCollectionUri("products", "subscriptions");
            var query = client.CreateDocumentQuery<Subscription>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(s => s.ProductId == product.ProductId)
                .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                foreach (Subscription subscription in await query.ExecuteNextAsync())
                {
                    if (subscription.LastPrice != product.Price)
                    {
                        await messages.AddAsync(new Message()
                        {
                            PhoneNumber = subscription.PhoneNumber,
                            Body = $"The price of the product '{product.Name}' is now {product.Price} Euro"
                        });
                        subscription.LastPrice = product.Price;
                        await subscriptions.AddAsync(subscription);
                    }
                }
            }
        }
    }
}
