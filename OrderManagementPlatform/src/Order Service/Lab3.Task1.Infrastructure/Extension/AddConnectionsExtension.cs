using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Infrastructure.Database.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Lab3.Task1.Infrastructure.Extension;

public static class AddConnectionsExtension
{
    public static IServiceCollection AddMyConnections(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddOptions<PostgresOptions>().Bind(configuration.GetSection("Postgres"));
        serviceCollection.AddSingleton<NpgsqlConnectionStringBuilder>();
        var builder = new NpgsqlDataSourceBuilder();
        builder.MapEnum<OrderState>(pgName: "order_state");
        builder.MapEnum<OrderHistoryItemKind>(pgName: "order_history_item_kind");
        serviceCollection.AddSingleton(builder);
        serviceCollection.AddSingleton<PostgresProvider>();
        return serviceCollection;
    }
}