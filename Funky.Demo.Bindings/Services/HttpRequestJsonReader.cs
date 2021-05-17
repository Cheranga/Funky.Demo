using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Funky.Demo.Services
{
    public class HttpRequestJsonReader : IHttpRequestJsonReader
    {
        public async Task<TModel> ReadModelAsync<TModel>(HttpRequestMessage request) where TModel : class, new()
        {
            var content = await request.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) return null;

            var model = JsonConvert.DeserializeObject<TModel>(content, new JsonSerializerSettings
            {
                Error = (sender, args) => args.ErrorContext.Handled = true
            });

            return model;
        }
    }
}