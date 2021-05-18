using System.Threading.Tasks;
using Funky.Demo.Messages;

namespace Funky.Demo.Functions
{
    public interface IHandleOrder
    {
        Task HandleOrderAsync(CreateOrderMessage message);
    }
}