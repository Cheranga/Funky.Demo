using System;
using System.Threading.Tasks;
using Funky.Demo.Messages;
using Microsoft.Azure.WebJobs;

namespace Funky.Demo.Functions
{
    public class CreateOrderFunction
    {
        [FunctionName(nameof(CreateOrderFunction))]
        public async Task CreateOrderAsync([QueueTrigger("%CustomerOrdersQueue%", Connection = "QueueConnectionString")]
            CreateOrderMessage message)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}