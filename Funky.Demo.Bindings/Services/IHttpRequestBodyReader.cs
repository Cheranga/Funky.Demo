using System.Net.Http;
using System.Threading.Tasks;

namespace Funky.Demo.Services
{
    public interface IHttpRequestBodyReader
    {
        Task<TModel> ReadModelAsync<TModel>(HttpRequestMessage request) where TModel : class, new();
    }
}