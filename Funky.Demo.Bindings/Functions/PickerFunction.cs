using System;
using System.Threading.Tasks;
using Funky.Demo.Abstractions;
using Funky.Demo.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Funky.Demo.Functions
{
    public class PickerFunction : IPicker
    {
        private readonly IDurableEntityContext context;
        private readonly ILogger<PickerFunction> logger;

        public PickerFunction(IDurableEntityContext context, ILogger<PickerFunction> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        
        [FunctionName(nameof(PickerFunction))]
        public static Task RunAsync([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<PickerFunction>(context);
        }

        public async Task PickAsync(PickOrderMessage pickOrder)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            
            logger.LogWarning("Picker {pickerId} at work {productCode} {quantity}", pickOrder.Id, pickOrder.ProductCode, pickOrder.Quantity);
        }
    }
}