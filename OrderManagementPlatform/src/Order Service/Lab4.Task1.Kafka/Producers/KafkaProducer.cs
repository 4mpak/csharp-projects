using Confluent.Kafka;
using Lab4.Task1.Kafka.Message;
using Lab4.Task1.Kafka.Options;
using Microsoft.Extensions.Options;

namespace Lab4.Task1.Kafka.Producers;

public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ProducerKafkaOptions _producerOptions;
    private readonly ConnectionKafkaOptions _connectionOptions;

    public KafkaProducer(IOptions<ProducerKafkaOptions> options, IOptions<ConnectionKafkaOptions> connectionOptions, ISerializer<TKey> key, ISerializer<TValue> value)
    {
        _producerOptions = options.Value;
        _connectionOptions = connectionOptions.Value;

        var config = new ProducerConfig
        {
            BootstrapServers = _connectionOptions.Host,
            SecurityProtocol = _connectionOptions.SecurityProtocol,
        };
        _producer = new ProducerBuilder<TKey, TValue>(config).SetKeySerializer(key).SetValueSerializer(value).Build();
    }

    public async Task ProduceAsync(ProducerMessage<TKey, TValue> message, CancellationToken cancellationToken)
    {
        var producerMessage = new Message<TKey, TValue>
        {
            Key = message.Key,
            Value = message.Value,
        };
        await _producer.ProduceAsync(_producerOptions.Topic, producerMessage, cancellationToken);
    }
}