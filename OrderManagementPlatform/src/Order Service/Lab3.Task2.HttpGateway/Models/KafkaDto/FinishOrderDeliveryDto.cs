namespace Lab3.Task2.HttpGateway.Models.KafkaDto;

public class FinishOrderDeliveryDto
{
    public required bool IsSaccessful { get; init; }

    public string? FailureReason { get; set; }
}