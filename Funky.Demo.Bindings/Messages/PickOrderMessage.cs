namespace Funky.Demo.Messages
{
    public class PickOrderMessage
    {
        public string OrderId { get; set; }
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public long Quantity { get; set; }
    }
}