using FluentValidation;
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

            RegisterMappers(services);
            RegisterValidators(services);
            RegisterServices(services);
        }

        private void RegisterMappers(IServiceCollection services)
        {
            var assemblies = new[]
            {
                typeof(Startup).Assembly
            };

            services.AddAutoMapper(assemblies);
        }

        private void RegisterValidators(IServiceCollection services)
        {
            var assemblies = new[]
            {
                typeof(Startup).Assembly
            };

            services.AddValidatorsFromAssemblies(assemblies);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ICreateProductService, CreateProductService>();
            services.AddSingleton<IHttpRequestBodyReader, HttpRequestBodyReader>();
            services.AddSingleton<IPickWorkerFactory, PickWorderFactory>();
            services.AddScoped<IHttpHeaderReader, HttpHeaderReader>();
        }
    }
}