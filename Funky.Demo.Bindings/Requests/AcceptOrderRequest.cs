using System.Collections.Generic;

namespace Funky.Demo.Requests
{
    public class AcceptOrderRequest
    {
        public string CorrelationId { get; set; }
        public string OrderId { get; set; }
        public string FlyBuysId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public string Category { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public long Quantity { get; set; }
    }
}