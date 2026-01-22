namespace Lab3.Task2.HttpGateway.Models.KafkaDto;

public class ApproveOrderDto
{
    public required bool IsApproved { get; init; }

    public required string ApprovedBy { get; init; }

    public string? FailureReason { get; init; }
}