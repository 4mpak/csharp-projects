namespace TrainRouteSimulator.General;

public abstract record Result
{
    public sealed record Success : Result;

    public sealed record SuccessWithTime(int Time) : Result;

    public sealed record ExceedingTheMaxPermissibleForce : Result;

    public sealed record ExceedingTheMaxPermissibleSpeed : Result;

    public sealed record LackOfSpeedAndAcceleration : Result;

    public sealed record ExceedingTheSpeedLimit : Result;
}