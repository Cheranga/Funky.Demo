using Funky.Demo;
using Funky.Demo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly:FunctionsStartup(typeof(Startup))]
namespace Funky.Demo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ICreateProductService, CreateProductService>();
            services.AddSingleton<IHttpRequestBodyReader, HttpRequestBodyReader>();
        }
    }
}