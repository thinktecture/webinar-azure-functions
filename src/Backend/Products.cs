using AzFnWebinar.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Backend
{
    public static class Products
    {
        [FunctionName("GetProducts")]
        public static IActionResult GetProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")]
            HttpRequest req,
            [CosmosDB(databaseName: "products", collectionName: "products", ConnectionStringSetting = "CosmosDBConnection")]
            IEnumerable<Product> products)
        {
            return new OkObjectResult(products);
        }

        [FunctionName("UpdateProduct")]
        public static IActionResult UpdateProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{productType}/{id}")]
            ProductInputModel inputModel,
            [CosmosDB(databaseName: "products", collectionName: "products",
                ConnectionStringSetting = "CosmosDBConnection", Id="{id}", PartitionKey = "{productType}")]
            Product existingProduct,
            ClaimsPrincipal user
            )
        {
            if (!user.IsAdmin())
            {
                return new UnauthorizedResult();
            }

            if (existingProduct == null)
            {
                return new NotFoundResult();
            }

            existingProduct.ProductId = inputModel.ProductId;
            existingProduct.Name = inputModel.Name;

            return new NoContentResult();
        }
    }
}
