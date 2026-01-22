using Confluent.Kafka;
using Lab4.Task1.Kafka.Message;
using Lab4.Task1.Kafka.Options;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace Lab4.Task1.Kafka.Consumers;

public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
{
    private readonly IDeserializer<TKey> _key;
    private readonly IDeserializer<TValue> _value;
    private readonly ConsumerKafkaOptions _consumerOptions;
    private readonly ConnectionKafkaOptions _connectionOptions;

    public KafkaConsumer(
        IDeserializer<TKey> key,
        IDeserializer<TValue> value,
        IOptions<ConsumerKafkaOptions> consumerOptions,
        IOptions<ConnectionKafkaOptions> connectionOptions)
    {
        _key = key;
        _value = value;
        _consumerOptions = consumerOptions.Value;
        _connectionOptions = connectionOptions.Value;
    }

    public async Task ReadAsync(ChannelWriter<ConsumerMessage<TKey, TValue>> writer, CancellationToken cancellationToken)
    {
        await Task.Yield();

        var config = new ConsumerConfig
        {
            BootstrapServers = _connectionOptions.Host,
            SecurityProtocol = _connectionOptions.SecurityProtocol,
            GroupId = _consumerOptions.Group,
            EnableAutoCommit = false,
        };
        using IConsumer<TKey, TValue> consumer = new ConsumerBuilder<TKey, TValue>(config).SetKeyDeserializer(_key).SetValueDeserializer(_value).Build();
        consumer.Subscribe(_consumerOptions.Topic);

        try
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                ConsumeResult<TKey, TValue> result = consumer.Consume(cancellationToken);
                var message = new ConsumerMessage<TKey, TValue>(consumer, result);
                await writer.WriteAsync(message, cancellationToken);
            }
        }
        finally
        {
            consumer.Close();
        }
    }
}