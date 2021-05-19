
using Microsoft.WindowsAzure.Storage.Table;
namespace Funky.Demo.Models
{
    public class OrderItemData : TableEntity
    {
        public string OrderId { get; set; }
        public string OrderData { get; set; }
    }
}