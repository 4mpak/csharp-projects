using Microsoft.Extensions.Options;
using Npgsql;

namespace Lab3.Task1.Infrastructure.Database.Connections;

public class PostgresProvider
{
    private readonly NpgsqlDataSource _npgsqlDataSource;

    public PostgresProvider(NpgsqlDataSourceBuilder npgsqlDataSourceBuilder, IOptionsMonitor<PostgresOptions> optionsMonitor)
    {
        PostgresOptions options = optionsMonitor.CurrentValue;
        NpgsqlConnectionStringBuilder connectionString = npgsqlDataSourceBuilder.ConnectionStringBuilder;
        connectionString.Host = options.Host;
        connectionString.Port = options.Port;
        connectionString.Username = options.User;
        connectionString.Password = options.Password;
        _npgsqlDataSource = npgsqlDataSourceBuilder.Build();
    }

    public async Task<NpgsqlConnection> ConnectAsync()
    {
        return await _npgsqlDataSource.OpenConnectionAsync();
    }

    public string GetConnectionString(NpgsqlDataSourceBuilder builder, PostgresOptions options)
    {
        NpgsqlConnectionStringBuilder connectionString = builder.ConnectionStringBuilder;
        connectionString.Host = options.Host;
        connectionString.Port = options.Port;
        connectionString.Username = options.User;
        connectionString.Password = options.Password;
        return connectionString.ToString();
    }
}