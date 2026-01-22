namespace Lab3.Task1.Infrastructure.Database.Connections;

public class PostgresOptions
{
    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Host { get; set; } = string.Empty;
}