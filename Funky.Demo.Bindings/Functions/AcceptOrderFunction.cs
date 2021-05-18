using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using Funky.Demo.Requests;
using Funky.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Funky.Demo.Functions
{
    public class AcceptOrderFunction
    {
        private readonly IHttpRequestBodyReader requestReader;
        private readonly IValidator<AcceptOrderRequest> validator;

        public AcceptOrderFunction(IHttpRequestBodyReader requestReader, IValidator<AcceptOrderRequest> validator)
        {
            this.requestReader = requestReader;
            this.validator = validator;
        }
        
        [FunctionName(nameof(AcceptOrderFunction))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
            HttpRequestMessage request)
        {
            var acceptOrderRequest = await requestReader.ReadModelAsync<AcceptOrderRequest>(request);
            var validationResult = await validator.ValidateAsync(acceptOrderRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult("Invalid request");
            }

            return new AcceptedResult();
        }
    }
}