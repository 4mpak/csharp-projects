using FluentMigrator.Runner;
using Lab3.Task1.Infrastructure.Database.Connections;
using Lab3.Task1.Infrastructure.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Lab3.Task1.Infrastructure.Extension;

public static class AddMigrationsExtension
{
    public static IServiceCollection AddMigrations(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(provider =>
                {
                    NpgsqlDataSourceBuilder builder = provider.GetRequiredService<NpgsqlDataSourceBuilder>();
                    PostgresOptions options = provider.GetRequiredService<IOptionsMonitor<PostgresOptions>>().CurrentValue;
                    PostgresProvider postgresProvider = provider.GetRequiredService<PostgresProvider>();
                    return postgresProvider.GetConnectionString(builder, options);
                })
                .WithMigrationsIn(typeof(InitialMigration).Assembly));
        return serviceCollection;
    }
}