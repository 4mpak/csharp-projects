using Lab3.Task1.Application.Constans.Services;
using Lab3.Task1.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab3.Task1.Application.Extension;

public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IOrderService, OrderService>();
        serviceCollection.AddScoped<IProductService, ProductService>();
        return serviceCollection;
    }
}