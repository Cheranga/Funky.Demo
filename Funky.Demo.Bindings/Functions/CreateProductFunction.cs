using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Funky.Demo.Models;
using Funky.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Funky.Demo.Functions
{
    public class CreateProductFunction
    {
        private readonly ICreateProductService createProductService;
        private readonly IHttpRequestBodyReader requestReader;

        public CreateProductFunction(IHttpRequestBodyReader requestReader, ICreateProductService createProductService)
        {
            this.requestReader = requestReader;
            this.createProductService = createProductService;
        }

        [FunctionName(nameof(CreateProductFunction))]
        [return:Queue("%CreateProductsQueue%",Connection = "QueueConnectionString")]
        public async Task<CreateProductCommand> CreateProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
            HttpRequestMessage request)
        {
            var createProductRequest = await requestReader.ReadModelAsync<CreateProductRequest>(request);
            if (createProductRequest == null)
            {
                return null;
            }

            var command = new CreateProductCommand
            {
                Id = createProductRequest.Id,
                Code = createProductRequest.Name,
                Name = createProductRequest.Name
            };

            return command;
        }
    }
}