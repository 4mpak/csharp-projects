using Confluent.Kafka;
using System.Diagnostics.CodeAnalysis;

namespace Lab4.Task1.Kafka.Message;

public class ConsumerMessage<TKey, TValue>
{
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly ConsumeResult<TKey, TValue> _result;

    [SetsRequiredMembers]
    public ConsumerMessage(IConsumer<TKey, TValue> consumer, ConsumeResult<TKey, TValue> result)
    {
        _consumer = consumer;
        _result = result;
        Key = result.Message.Key;
        Value = result.Message.Value;
    }

    public required TKey Key { get; init; }

    public required TValue Value { get; init; }

    public void Commit()
    {
        _consumer.Commit(_result);
    }
}