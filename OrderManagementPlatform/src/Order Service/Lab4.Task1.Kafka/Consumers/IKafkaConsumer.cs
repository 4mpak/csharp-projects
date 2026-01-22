using Lab4.Task1.Kafka.Message;
using System.Threading.Channels;

namespace Lab4.Task1.Kafka.Consumers;

public interface IKafkaConsumer<TKey, TValue>
{
    Task ReadAsync(ChannelWriter<ConsumerMessage<TKey, TValue>> writer, CancellationToken cancellationToken);
}