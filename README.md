## Azure function bindings

> Dependency Injection

```c#
using Funky.Demo;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly:FunctionsStartup(typeof(Startup))]
namespace Funky.Demo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            // TODO: Register the dependencies
        }
    }
}
```

> Azure function bindings

https://docs.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings?tabs=csharp#supported-bindings


