using System.Collections.Generic;
using Funky.Demo.Requests;

namespace Funky.Demo.Messages
{
    public class CreateOrderMessage
    {
        public string CorrelationId { get; set; }
        public string OrderId { get; set; }
        public string LoyaltyId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}