using System.Net.Http;
using System.Threading.Tasks;

namespace Funky.Demo.Services
{
    public interface IHttpRequestJsonReader
    {
        Task<TModel> ReadModelAsync<TModel>(HttpRequestMessage request) where TModel : class, new();
    }
}