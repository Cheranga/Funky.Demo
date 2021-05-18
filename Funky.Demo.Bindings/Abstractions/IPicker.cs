using System.Threading.Tasks;
using Funky.Demo.Messages;

namespace Funky.Demo.Abstractions
{
    public interface IPicker
    {
        Task PickAsync(PickOrderMessage pickOrder);
    }
}