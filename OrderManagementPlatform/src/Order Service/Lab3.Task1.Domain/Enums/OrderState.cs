using NpgsqlTypes;

namespace Lab3.Task1.Domain.Enums;

public enum OrderState
{
    [PgName("created")]
    Created,
    [PgName("processing")]
    Processing,
    [PgName("completed")]
    Completed,
    [PgName("cancelled")]
    Cancelled,
}