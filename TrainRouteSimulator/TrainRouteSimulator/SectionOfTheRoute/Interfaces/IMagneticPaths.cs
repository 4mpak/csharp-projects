using TrainRouteSimulator.General;
using TrainRouteSimulator.Trains;

namespace TrainRouteSimulator.SectionOfTheRoute.Interfaces;

public interface IMagneticPaths
{
    Result CalculationOfDuration(ITrain train);
}