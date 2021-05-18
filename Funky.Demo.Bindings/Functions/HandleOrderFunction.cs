using System;
using System.Threading.Tasks;
using Funky.Demo.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Demo.Functions
{
    public class HandleOrderFunction : IHandleOrder
    {
        private readonly IDurableEntityContext context;

        public HandleOrderFunction(IDurableEntityContext context)
        {
            this.context = context;
        }

        [FunctionName(nameof(HandleOrderFunction))]
        public static Task RunAsync([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<HandleOrderFunction>(context);
        }

        public async Task HandleOrderAsync(CreateOrderMessage message)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}