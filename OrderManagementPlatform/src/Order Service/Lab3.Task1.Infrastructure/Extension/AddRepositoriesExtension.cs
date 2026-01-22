using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Infrastructure.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lab3.Task1.Infrastructure.Extension;

public static class AddRepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddScoped<IOrderItemRepository, OrderItemRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        return serviceCollection;
    }
}