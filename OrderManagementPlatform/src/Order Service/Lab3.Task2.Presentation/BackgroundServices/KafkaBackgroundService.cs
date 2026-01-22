using Lab3.Task2.Presentation.Services.Kafka.Interfaces;
using Lab4.Task1.Kafka.Consumers;
using Lab4.Task1.Kafka.Message;
using Lab4.Task1.Kafka.Options;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace Lab3.Task2.Presentation.BackgroundServices;

public class KafkaBackgroundService<TKey, TValue> : BackgroundService
{
    private readonly Channel<ConsumerMessage<TKey, TValue>> _channel;
    private readonly IServiceScopeFactory _factory;

    public KafkaBackgroundService(IOptions<ConsumerKafkaOptions> options, IServiceScopeFactory factory)
    {
        _factory = factory;
        var optionsChannel = new BoundedChannelOptions(options.Value.SizeBatch * 3)
        {
            SingleReader = true,
            SingleWriter = true,
            FullMode = BoundedChannelFullMode.Wait,
        };
        _channel = Channel.CreateBounded<ConsumerMessage<TKey, TValue>>(optionsChannel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope scope = _factory.CreateAsyncScope();
        IKafkaConsumer<TKey, TValue> consumer = scope.ServiceProvider.GetRequiredService<IKafkaConsumer<TKey, TValue>>();
        IKafkaConsumerHandler<TKey, TValue> handler = scope.ServiceProvider.GetRequiredService<IKafkaConsumerHandler<TKey, TValue>>();
        Task taskConsumer = consumer.ReadAsync(_channel.Writer, stoppingToken);
        Task taskHandler = handler.HandleAsync(_channel.Reader, stoppingToken);
        await Task.WhenAll(taskConsumer, taskHandler);
    }
}