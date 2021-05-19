using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Funky.Demo.Services
{
    public interface IHttpHeaderReader
    {
        string GetHeader(HttpRequestMessage request, string header);
    }
    
    public class HttpHeaderReader : IHttpHeaderReader
    {
        public string GetHeader(HttpRequestMessage request, string header)
        {
            if (request?.Headers != null && request.Headers.TryGetValues(header, out var headerData))
            {
                return headerData.FirstOrDefault();
            }

            return string.Empty;
        }
    }

    public interface IHttpRequestBodyReader
    {
        Task<TModel> ReadModelAsync<TModel>(HttpRequestMessage request) where TModel : class, new();
    }
}