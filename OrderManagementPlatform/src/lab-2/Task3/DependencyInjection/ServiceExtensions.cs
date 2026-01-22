using Microsoft.Extensions.DependencyInjection;
using Task3.Options;
using Task3.Renderers;
using Task3.Service;

namespace Task3.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddDisplayService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<DisplayOptions>().BindConfiguration("Display");
        serviceCollection.AddSingleton<IRenderer, Renderer>();
        serviceCollection.AddSingleton<IConsoleService, ConsoleService>();
        return serviceCollection;
    }
}