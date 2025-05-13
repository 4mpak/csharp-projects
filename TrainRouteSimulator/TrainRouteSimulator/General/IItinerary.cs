using TrainRouteSimulator.SectionOfTheRoute.Interfaces;
using TrainRouteSimulator.Trains;

namespace TrainRouteSimulator.General;

public interface IItinerary
{
    void AddMagneticPath(IMagneticPaths magneticPaths);

    Result PassingTheRoute(ITrain train);
}