using System;
using System.Threading.Tasks;
using Funky.Demo.Abstractions;
using Funky.Demo.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Funky.Demo.Functions
{
    public class CreateOrderFunction
    {
        private readonly ILogger<CreateOrderFunction> logger;

        public CreateOrderFunction(ILogger<CreateOrderFunction> logger)
        {
            this.logger = logger;
        }
        
        [FunctionName(nameof(CreateOrderFunction))]
        public async Task CreateOrderAsync([QueueTrigger("%CustomerOrdersQueue%", Connection = "QueueConnectionString")]
            CreateOrderMessage message, [DurableClient]IDurableClient client)
        {
            logger.LogInformation("{correlationId} starting to process order", message.CorrelationId);
            var entityId = new EntityId(nameof(HandleOrderFunction), message.OrderId.ToUpper());
            await client.SignalEntityAsync<IHandleOrder>(entityId, handler => handler.ExecuteAsync(message));
        }
    }
}