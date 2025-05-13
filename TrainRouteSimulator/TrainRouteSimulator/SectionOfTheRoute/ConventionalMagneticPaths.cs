using TrainRouteSimulator.General;
using TrainRouteSimulator.SectionOfTheRoute.Interfaces;
using TrainRouteSimulator.Trains;

namespace TrainRouteSimulator.SectionOfTheRoute;

public class ConventionalMagneticPaths(int distance) : IMagneticPaths
{
    private int Distance { get; } = distance;

    public Result CalculationOfDuration(ITrain train)
    {
        train.Acceleration = 0;
        return train.CalculationOfDuration(Distance);
    }
}