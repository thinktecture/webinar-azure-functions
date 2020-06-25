using AzFnWebinar.Shared;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend
{
    public static class EventGridBridge
    {
        [FunctionName("EventGridBridge")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "products",
            collectionName: "products",
            ConnectionStringSetting = "CosmosDBConnection",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [EventGrid(TopicEndpointUri = "products-topic", TopicKeySetting = "products-topic-key")]
                IAsyncCollector<EventGridEvent> outputEvents)
        {
            if (input == null || input.Count <= 0)
            {
                return;
            }

            foreach (var item in input)
            {
                var product = JsonConvert.DeserializeObject<Product>(item.ToString());
                await outputEvents.AddAsync(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "productUpdated",
                    Subject = item.SelfLink,
                    Data = product,
                    DataVersion = "1.0",
                    EventTime = DateTime.UtcNow
                });
            }
        }
    }
}
