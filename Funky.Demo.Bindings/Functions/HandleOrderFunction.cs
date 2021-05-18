using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funky.Demo.Abstractions;
using Funky.Demo.Messages;
using Funky.Demo.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Demo.Functions
{
    public class HandleOrderFunction : IHandleOrder
    {
        private readonly IDurableEntityContext context;
        private readonly IPickWorkerFactory pickWorkerFactory;

        public Dictionary<string, PickOrderData> OrderItemsMappedByProductCode { get; set; }

        public HandleOrderFunction(IDurableEntityContext context, IPickWorkerFactory pickWorkerFactory)
        {
            this.context = context;
            this.pickWorkerFactory = pickWorkerFactory;
            this.OrderItemsMappedByProductCode = new Dictionary<string, PickOrderData>();
        }

        [FunctionName(nameof(HandleOrderFunction))]
        public static Task HandleOrderAsync([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<HandleOrderFunction>(context);
        }

        public Task ExecuteAsync(CreateOrderMessage message)
        {
            Reset();

            SetupOrderItems(message);
            
            InitiateOrderPicking();

            return Task.CompletedTask;
        }

        private void Reset()
        {
            OrderItemsMappedByProductCode.Clear();
        }

        private void InitiateOrderPicking()
        {
            foreach (var (id, item) in OrderItemsMappedByProductCode)
            {
                var pickerData = pickWorkerFactory.GetPicker(item.OrderItem.Category);
                var entityId = new EntityId(pickerData.Category, pickerData.Id);

                var pickOrderMessage = new PickOrderMessage
                {
                    Id = id,
                    ProductCode = item.OrderItem.ProductCode,
                    Quantity = item.OrderItem.Quantity
                };

                context.SignalEntity<IPicker>(entityId, picker => picker.PickAsync(pickOrderMessage));
            }
        }

        private void SetupOrderItems(CreateOrderMessage message)
        {
            message.OrderItems.ForEach(x =>
            {
                var pickOrderData = new PickOrderData
                {
                    Id = $"{message.OrderId}-{x.ProductCode}-{Guid.NewGuid():N}".ToUpper(),
                    OrderItem = x,
                    Picked = false
                };

                OrderItemsMappedByProductCode.Add(pickOrderData.Id, pickOrderData);
            });
        }
    }
}