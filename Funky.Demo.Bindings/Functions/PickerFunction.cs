using System;
using System.Threading.Tasks;
using Funky.Demo.Abstractions;
using Funky.Demo.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Funky.Demo.Functions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PickerFunction : IPicker
    {
        private readonly IDurableEntityContext context;
        private readonly ILogger<PickerFunction> logger;
        private readonly Random random;

        public PickerFunction(IDurableEntityContext context, ILogger<PickerFunction> logger)
        {
            this.context = context;
            this.logger = logger;

            this.random = new Random();
        }
        
        [FunctionName(nameof(PickerFunction))]
        public static Task RunAsync([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<PickerFunction>(context);
        }

        public async Task PickAsync(PickOrderMessage pickOrder)
        {
            PickOrder = pickOrder;
            logger.LogInformation("{correlationId} Picker {pickerId} at work {productCode} {quantity}", pickOrder.CorrelationId, pickOrder.Id, pickOrder.ProductCode, pickOrder.Quantity);
            
            var delay = TimeSpan.FromSeconds(random.Next(1, 6));
            await Task.Delay(delay);

            await CompleteAsync();
        }

        [JsonProperty]
        public PickOrderMessage PickOrder { get; set; }

        private Task CompleteAsync()
        {
            var entityId = new EntityId(nameof(HandleOrderFunction), PickOrder.OrderId);
            
            logger.LogInformation("{correlationId} {pickerId} completed {productCode} {quantity}", PickOrder.CorrelationId, PickOrder.Id, PickOrder.ProductCode, PickOrder.Quantity);
            context.SignalEntity<IHandleOrder>(entityId, x=>x.AcknowledgePickComplete(PickOrder.Id));

            return Task.CompletedTask;
        }
    }
}