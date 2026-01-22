using NpgsqlTypes;

namespace Lab3.Task1.Domain.Enums;

public enum OrderHistoryItemKind
{
    [PgName("created")]
    Created,
    [PgName("item_added")]
    ItemAdded,
    [PgName("item_removed")]
    ItemRemoved,
    [PgName("state_changed")]
    StateChanged,
}