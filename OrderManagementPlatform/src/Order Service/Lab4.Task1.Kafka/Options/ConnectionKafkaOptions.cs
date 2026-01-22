using Confluent.Kafka;

namespace Lab4.Task1.Kafka.Options;

public class ConnectionKafkaOptions
{
    public required string Host { get; init; }

    public required SecurityProtocol SecurityProtocol { get; init; }
}