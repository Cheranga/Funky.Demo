using System.Threading.Tasks;
using Funky.Demo.Messages;

namespace Funky.Demo.Abstractions
{
    public interface IHandleOrder
    {
        Task ExecuteAsync(CreateOrderMessage message);
    }
}