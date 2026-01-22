using Lab3.Task2.Presentation.Services.Kafka.Interfaces;
using Lab4.Task1.Kafka.Message;
using Lab4.Task1.Kafka.Options;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace Lab3.Task2.Presentation.Services.Kafka;

public class ConsumerHandler<TKey, TValue> : IKafkaConsumerHandler<TKey, TValue>
{
    private readonly ConsumerKafkaOptions _options;
    private readonly IKafkaMessageBatch<TKey, TValue> _messageBatch;

    public ConsumerHandler(IOptions<ConsumerKafkaOptions> options, IKafkaMessageBatch<TKey, TValue> messageBatch)
    {
        _options = options.Value;
        _messageBatch = messageBatch;
    }

    public async Task HandleAsync(
        ChannelReader<ConsumerMessage<TKey, TValue>> reader,
        CancellationToken cancellationToken)
    {
        await Task.Yield();
        IAsyncEnumerable<IReadOnlyList<ConsumerMessage<TKey, TValue>>> messages = reader.ReadAllAsync(cancellationToken)
            .ChunkAsync(_options.SizeBatch, _options.LimitWaitBuffer);
        await foreach (IReadOnlyList<ConsumerMessage<TKey, TValue>> batch in messages)
        {
            ConsumerMessage<TKey, TValue>[] consumerMessages = batch.Where(x => x.Value != null).ToArray();
            await _messageBatch.HandleBatchMessage(batch, cancellationToken);
            foreach (ConsumerMessage<TKey, TValue> message in consumerMessages)
            {
                message.Commit();
            }
        }
    }
}