using Lab3.Task2.Presentation.Services.Kafka;
using Lab3.Task2.Presentation.Services.Kafka.Interfaces;
using Orders.Kafka.Contracts;

namespace Lab3.Task2.Presentation.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddConsumer(this IServiceCollection services)
    {
        services.AddScoped<IKafkaMessageBatch<OrderProcessingKey, OrderProcessingValue>,
            OrderProcessingHandler>();
        services.AddScoped<IKafkaConsumerHandler<OrderProcessingKey, OrderProcessingValue>,
        ConsumerHandler<OrderProcessingKey, OrderProcessingValue>>();
        return services;
    }
}