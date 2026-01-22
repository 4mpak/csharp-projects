using Lab4.Task1.Kafka.Message;
using System.Threading.Channels;

namespace Lab3.Task2.Presentation.Services.Kafka.Interfaces;

public interface IKafkaConsumerHandler<TKey, TValue>
{
    Task HandleAsync(ChannelReader<ConsumerMessage<TKey, TValue>> reader, CancellationToken cancellationToken);
}