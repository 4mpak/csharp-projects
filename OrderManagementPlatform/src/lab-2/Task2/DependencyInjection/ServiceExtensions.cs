using Microsoft.Extensions.DependencyInjection;
using Task2.Interfaces;
using Task2.Options;
using Task2.Service;

namespace Task2.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<ServiceOptions>().BindConfiguration("ServiceOptions");
        serviceCollection.AddSingleton<IConfigurationsService, ConfigurationsService>();
        return serviceCollection;
    }
}