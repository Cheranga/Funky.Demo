using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Funky.Demo.Messages;
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
        private readonly IMapper mapper;

        public AcceptOrderFunction(IHttpRequestBodyReader requestReader, IValidator<AcceptOrderRequest> validator, IMapper mapper)
        {
            this.requestReader = requestReader;
            this.validator = validator;
            this.mapper = mapper;
        }
        
        [FunctionName(nameof(AcceptOrderFunction))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
            HttpRequestMessage request,
            [Queue("%CustomerOrdersQueue%", Connection = "QueueConnectionString")]IAsyncCollector<CreateOrderMessage> messages)
        {
            var acceptOrderRequest = await requestReader.ReadModelAsync<AcceptOrderRequest>(request);
            var validationResult = await validator.ValidateAsync(acceptOrderRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult("Invalid request");
            }

            var message = mapper.Map<CreateOrderMessage>(acceptOrderRequest);
            await messages.AddAsync(message);
            
            return new AcceptedResult();
        }
    }
}