using Lab4.Task1.Kafka.Message;

namespace Lab3.Task2.Presentation.Services.Kafka.Interfaces;

public interface IKafkaMessageBatch<TKey, TValue>
{
    Task HandleBatchMessage(IEnumerable<ConsumerMessage<TKey, TValue>> message, CancellationToken cancellationToken);
}