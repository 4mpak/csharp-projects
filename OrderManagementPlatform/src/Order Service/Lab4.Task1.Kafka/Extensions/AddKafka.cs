using Confluent.Kafka;
using Google.Protobuf;
using Lab4.Task1.Kafka.Consumers;
using Lab4.Task1.Kafka.Options;
using Lab4.Task1.Kafka.Producers;
using Lab4.Task1.Kafka.Serializer;
using Microsoft.Extensions.DependencyInjection;

namespace Lab4.Task1.Kafka.Extensions;

public static class AddKafka
{
    public static IServiceCollection AddKafkaExtension(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<ConnectionKafkaOptions>().BindConfiguration("Kafka:Connection");
        return serviceCollection;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection serviceCollection)
    where TKey : IMessage<TKey>, new()
    where TValue : IMessage<TValue>, new()
    {
        serviceCollection.AddOptions<ProducerKafkaOptions>().BindConfiguration("Kafka:Producer");
        serviceCollection.AddSingleton<ISerializer<TKey>, KafkaSerializer<TKey>>();
        serviceCollection.AddSingleton<ISerializer<TValue>, KafkaSerializer<TValue>>();
        serviceCollection.AddScoped<IKafkaProducer<TKey, TValue>, KafkaProducer<TKey, TValue>>();
        return serviceCollection;
    }

    public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection serviceCollection)
        where TKey : IMessage<TKey>, new()
        where TValue : IMessage<TValue>, new()
    {
        serviceCollection.AddOptions<ConsumerKafkaOptions>().BindConfiguration("Kafka:Consumer");
        serviceCollection.AddSingleton<IDeserializer<TKey>, KafkaSerializer<TKey>>();
        serviceCollection.AddSingleton<IDeserializer<TValue>, KafkaSerializer<TValue>>();
        serviceCollection.AddScoped<IKafkaConsumer<TKey, TValue>, KafkaConsumer<TKey, TValue>>();
        return serviceCollection;
    }
}