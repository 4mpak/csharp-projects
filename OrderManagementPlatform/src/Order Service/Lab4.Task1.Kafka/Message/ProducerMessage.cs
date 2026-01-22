namespace Lab4.Task1.Kafka.Message;

public record ProducerMessage<TKey, TValue>(TKey Key, TValue Value);