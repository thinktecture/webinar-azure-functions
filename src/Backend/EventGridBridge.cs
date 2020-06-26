using AzFnWebinar.Backend.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzFnWebinar.Backend
{
    public class EventGridBridge
    {
        [FunctionName("EventGridBridge")]
        public async Task Run(
            [CosmosDBTrigger("products", "products", ConnectionStringSetting = "CosmosDBConnection")]
                IReadOnlyList<Document> input,
            [EventGrid(TopicEndpointUri = "products-topic", TopicKeySetting = "products-topic-key")]
                IAsyncCollector<EventGridEvent> outputEvents,
            ILogger log)
        {
            if (input == null || input.Count <= 0)
            {
                log.LogInformation("No inputs to publish.");
                return;
            }

            log.LogInformation("Publishing {count} changes.", input.Count);

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
