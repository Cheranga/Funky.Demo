using System;
using System.Threading.Tasks;
using Funky.Demo.Abstractions;
using Funky.Demo.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Demo.Functions
{
    public class CreateOrderFunction
    {
        [FunctionName(nameof(CreateOrderFunction))]
        public async Task CreateOrderAsync([QueueTrigger("%CustomerOrdersQueue%")]
            CreateOrderMessage message, [DurableClient]IDurableClient client)
        {
            var entityId = new EntityId(nameof(HandleOrderFunction), message.OrderId.ToUpper());
            await client.SignalEntityAsync<IHandleOrder>(entityId, handler => handler.ExecuteAsync(message));
        }
    }
}