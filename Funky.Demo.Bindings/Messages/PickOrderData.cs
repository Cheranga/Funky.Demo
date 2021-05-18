using Funky.Demo.Requests;

namespace Funky.Demo.Messages
{
    public class PickOrderData
    {
        public string Id { get; set; }
        public OrderItem OrderItem { get; set; }
        public bool Picked { get; set; }
    }
}