using Lab4.Task1.Kafka.Message;

namespace Lab4.Task1.Kafka.Producers;

public interface IKafkaProducer<TKey, TValue>
{
    Task ProduceAsync(ProducerMessage<TKey, TValue> message, CancellationToken cancellationToken);
}