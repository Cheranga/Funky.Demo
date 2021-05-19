using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funky.Demo.Abstractions;
using Funky.Demo.Messages;
using Funky.Demo.Models;
using Funky.Demo.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Funky.Demo.Functions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HandleOrderFunction : IHandleOrder
    {
        private readonly IDurableEntityContext context;
        private readonly ILogger<HandleOrderFunction> logger;
        private readonly IPickWorkerFactory pickWorkerFactory;
        private readonly IAsyncCollector<ITableEntity> orders;

        public HandleOrderFunction(IDurableEntityContext context, IPickWorkerFactory pickWorkerFactory, IAsyncCollector<ITableEntity> orders, ILogger<HandleOrderFunction> logger)
        {
            this.context = context;
            this.pickWorkerFactory = pickWorkerFactory;
            this.orders = orders;
            this.logger = logger;
            OrderItemsMappedByProductCode = new Dictionary<string, PickOrderData>();
        }

        [JsonProperty]
        public Dictionary<string, PickOrderData> OrderItemsMappedByProductCode { get; set; }

        [JsonProperty]
        public CreateOrderMessage Order { get; set; }

        public Task ExecuteAsync(CreateOrderMessage message)
        {
            Order = message;

            SetupOrderItems(message);

            InitiateOrderPicking();

            return Task.CompletedTask;
        }

        public async Task AcknowledgePickComplete(string pickId)
        {
            if (OrderItemsMappedByProductCode.TryGetValue(pickId, out var pickOrderData))
            {
                pickOrderData.Picked = true;
            }

            await SaveOrderData(pickOrderData);

            var isInProgress = OrderItemsMappedByProductCode.Any(x => x.Value.Picked == false);

            if (!isInProgress)
            {
                Reset();
                logger.LogInformation("{correlationId} Order completed for order: {orderId}", Order.CorrelationId, Order.OrderId);
            }
        }

        private async Task SaveOrderData(PickOrderData pickOrderData)
        {
            var orderItem = new OrderItemData
            {
                PartitionKey = $"{pickOrderData.OrderId}".ToUpper(),
                RowKey = $"{pickOrderData.Id}".ToUpper(),
                OrderId = Order.OrderId,
                OrderData = JsonConvert.SerializeObject(pickOrderData)
            };

            logger.LogInformation("{correlationId} order item picked and ready {orderItemId}", Order.CorrelationId, pickOrderData.Id);
            await orders.AddAsync(orderItem);
        }

        [FunctionName(nameof(HandleOrderFunction))]
        public static Task HandleOrderAsync([EntityTrigger] IDurableEntityContext context,
            [Table("%OrdersTable%")]IAsyncCollector<ITableEntity> orders)
        {
            return context.DispatchAsync<HandleOrderFunction>(context, orders);
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
                    CorrelationId = Order.CorrelationId,
                    OrderId = Order.OrderId,
                    Id = id,
                    ProductCode = item.OrderItem.ProductCode,
                    Quantity = item.OrderItem.Quantity
                };

                logger.LogInformation("{correlationId} starting order picking for {orderItemId}", Order.CorrelationId, pickOrderMessage.Id);
                context.SignalEntity<IPicker>(entityId, picker => picker.PickAsync(pickOrderMessage));
            }
        }

        private void SetupOrderItems(CreateOrderMessage message)
        {
            message.OrderItems.ForEach(x =>
            {
                var pickOrderData = new PickOrderData
                {
                    OrderId = message.OrderId,
                    Id = $"{message.OrderId}-{x.ProductCode}-{Guid.NewGuid():N}".ToUpper(),
                    OrderItem = x,
                    Picked = false
                };

                OrderItemsMappedByProductCode.Add(pickOrderData.Id, pickOrderData);
            });
        }
    }
}