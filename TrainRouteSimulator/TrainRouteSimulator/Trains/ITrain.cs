using TrainRouteSimulator.General;

namespace TrainRouteSimulator.Trains;

public interface ITrain
{
    int Weight { get; set; }

    int Speed { get; set; }

    int Acceleration { get; set; }

    int MaximumPermissibleForce { get; set; }

    public Result CalculationOfDuration(int distance);
}