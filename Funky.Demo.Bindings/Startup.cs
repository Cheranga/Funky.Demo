using FluentValidation;
using Funky.Demo;
using Funky.Demo.CustomBindings;
using Funky.Demo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Funky.Demo
{
    public class Startup : IWebJobsStartup
    {
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

        public void Configure(IWebJobsBuilder builder)
        {
            builder.UseAzureAdTokenBinding();
            
            var services = builder.Services;

            RegisterMappers(services);
            RegisterValidators(services);
            RegisterServices(services);
        }
        
        protected virtual IConfigurationRoot GetConfigurationRoot(IWebJobsBuilder builder)
        {
            var services = builder.Services;

            var executionContextOptions = services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(executionContextOptions.AppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}