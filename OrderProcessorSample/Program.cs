using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProcessorSample.Repositories;
using OrderProcessorSample.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddScoped<OrderMessageProcessor>();
    })
    .Build();

host.Run();
