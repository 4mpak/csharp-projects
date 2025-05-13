using TrainRouteSimulator.General;
using TrainRouteSimulator.SectionOfTheRoute.Interfaces;
using TrainRouteSimulator.Trains;

namespace TrainRouteSimulator.SectionOfTheRoute;

public class MagneticForcePaths(int distance, int force) : IMagneticPaths
{
    private int Distance { get; } = distance;

    private int Force { get; } = force;

    public Result CalculationOfDuration(ITrain train)
    {
        if (Force > train.MaximumPermissibleForce)
        {
            return new Result.ExceedingTheMaxPermissibleForce();
        }

        train.Acceleration = Force / train.Weight;
        return train.CalculationOfDuration(Distance);
    }
}