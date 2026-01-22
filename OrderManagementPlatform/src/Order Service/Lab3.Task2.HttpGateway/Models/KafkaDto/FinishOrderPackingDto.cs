namespace Lab3.Task2.HttpGateway.Models.KafkaDto;

public class FinishOrderPackingDto
{
    public required bool IsSuccessful { get; init; }

    public string? FailureReason { get; init; }
}