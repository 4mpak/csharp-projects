using TrainRouteSimulator.General;

namespace TrainRouteSimulator.Trains;

public class Train(int weight, int maximumPermissibleForce, int accuracy) : ITrain
{
    public int Weight { get; set; } = weight;

    public int Speed { get; set; } = 0;

    public int Acceleration { get; set; } = 0;

    public int MaximumPermissibleForce { get; set; } = maximumPermissibleForce;

    public int Accuracy { get; set; } = accuracy;

    public Result CalculationOfDuration(int distance)
    {
        int duration = 0;
        while (distance > 0)
        {
            Speed = CalculationOfResultingSpeed();
            distance -= CalculationOfDistanceTraveled();
            duration += Accuracy;
            if (Speed <= 0 && Acceleration <= 0) return new Result.LackOfSpeedAndAcceleration();
        }

        return new Result.SuccessWithTime(duration);
    }

    public int CalculationOfDistanceTraveled()
    {
        DistanceTraveled = ResultingSpeed * Accuracy;
        return DistanceTraveled;
    }

    private int ResultingSpeed { get; set; }

    private int DistanceTraveled { get; set; }

    private int CalculationOfResultingSpeed()
    {
        ResultingSpeed = Speed + (Acceleration * Accuracy);
        return ResultingSpeed;
    }
}