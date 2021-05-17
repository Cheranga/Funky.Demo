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
        private readonly IHttpRequestJsonReader requestReader;

        public CreateProductFunction(IHttpRequestJsonReader requestReader, ICreateProductService createProductService)
        {
            this.requestReader = requestReader;
            this.createProductService = createProductService;
        }

        [FunctionName(nameof(CreateProductFunction))]
        public async Task<IActionResult> CreateProductAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")]
            HttpRequestMessage request)
        {
            var createProductRequest = await requestReader.ReadModelAsync<CreateProductRequest>(request);
            var operation = await createProductService.ExecuteAsync(createProductRequest);

            if (!operation.Status)
                return new ObjectResult(operation.Error)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };

            return new AcceptedResult();
        }
    }
}