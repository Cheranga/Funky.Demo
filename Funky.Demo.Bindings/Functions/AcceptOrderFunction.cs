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
        private readonly IHttpHeaderReader httpHeaderReader;

        public AcceptOrderFunction(IHttpRequestBodyReader requestReader, IValidator<AcceptOrderRequest> validator, IMapper mapper, IHttpHeaderReader httpHeaderReader)
        {
            this.requestReader = requestReader;
            this.validator = validator;
            this.mapper = mapper;
            this.httpHeaderReader = httpHeaderReader;
        }
        
        [FunctionName(nameof(AcceptOrderFunction))]
        public async Task<IActionResult> AcceptOrderAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
            HttpRequestMessage request,
            [Queue("%CustomerOrdersQueue%", Connection = "QueueConnectionString")]IAsyncCollector<CreateOrderMessage> messages)
        {
            var acceptOrderRequest = await requestReader.ReadModelAsync<AcceptOrderRequest>(request);
            var correlationId = httpHeaderReader.GetHeader(request, "correlationId");
            if (acceptOrderRequest != null)
            {
                acceptOrderRequest.CorrelationId = correlationId;
            }
            
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