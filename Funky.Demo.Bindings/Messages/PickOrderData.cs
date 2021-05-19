using Funky.Demo.Requests;

namespace Funky.Demo.Messages
{
    public class PickOrderData
    {
        public string OrderId { get; set; }
        public string Id { get; set; }
        public OrderItem OrderItem { get; set; }
        public bool Picked { get; set; }
    }
}