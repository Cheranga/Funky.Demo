using System.Threading.Tasks;
using Funky.Demo.Models;

namespace Funky.Demo.Services
{
    public interface ICreateProductService
    {
        Task<Result> ExecuteAsync(CreateProductRequest request);
    }
}