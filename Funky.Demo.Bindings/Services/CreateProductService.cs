using System;
using System.Threading.Tasks;
using Funky.Demo.Models;

namespace Funky.Demo.Services
{
    public class CreateProductService : ICreateProductService
    {
        public async Task<Result> ExecuteAsync(CreateProductRequest request)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return Result.Success();
        }
    }
}