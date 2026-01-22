using Lab3.Task2.HttpGateway.Options;
using Lab3.Task2.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lab3.Task2.HttpGateway.Extensions;

public static class GrpcClients
{
    public static IServiceCollection AddClients(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<ProductServiceOption>().BindConfiguration("MyClients");
        serviceCollection.AddOptions<OrderServiceOption>().BindConfiguration("MyClients");
        serviceCollection.AddGrpcClient<OrderServiceProto.OrderServiceProtoClient>((sp, o) =>
        {
            IOptions<OrderServiceOption> options = sp.GetRequiredService<IOptions<OrderServiceOption>>();
            o.Address = new Uri(options.Value.Link);
        });
        serviceCollection.AddGrpcClient<ProductServiceProto.ProductServiceProtoClient>((sp, o) =>
        {
            IOptions<ProductServiceOption> options = sp.GetRequiredService<IOptions<ProductServiceOption>>();
            o.Address = new Uri(options.Value.Link);
        });
        return serviceCollection;
    }
}