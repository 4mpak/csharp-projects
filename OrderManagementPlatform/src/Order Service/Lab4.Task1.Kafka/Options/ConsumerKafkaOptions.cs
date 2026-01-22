namespace Lab4.Task1.Kafka.Options;

public class ConsumerKafkaOptions
{
    public required string Topic { get; init; }

    public required string Group { get; init; }

    public required TimeSpan LimitWaitBuffer { get; init; }

    public required int SizeBatch { get; init; }
}