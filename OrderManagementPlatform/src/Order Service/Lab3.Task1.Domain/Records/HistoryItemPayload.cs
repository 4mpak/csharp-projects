using System.Text.Json.Serialization;

namespace Lab3.Task1.Domain.Records;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(HistoryItemPayloadCreated), typeDiscriminator: nameof(HistoryItemPayloadCreated))]
[JsonDerivedType(typeof(HistoryItemPayloadItemAdded), typeDiscriminator: nameof(HistoryItemPayloadItemAdded))]
[JsonDerivedType(typeof(HistoryItemPayloadItemRemoved), typeDiscriminator: nameof(HistoryItemPayloadItemRemoved))]
[JsonDerivedType(typeof(HistoryItemPayloadStateChanged), typeDiscriminator: nameof(HistoryItemPayloadStateChanged))]
public abstract record HistoryItemPayload();