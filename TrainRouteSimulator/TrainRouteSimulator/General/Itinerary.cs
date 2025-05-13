using System.Collections.Generic;
using System.Linq;
using TrainRouteSimulator.SectionOfTheRoute.Interfaces;
using TrainRouteSimulator.Trains;

namespace TrainRouteSimulator.General;

public class Itinerary(int maxSpeedInTheEnd) : IItinerary
{
    private int MaxSpeedInTheEnd { get; } = maxSpeedInTheEnd;

    private IEnumerable<IMagneticPaths> _sections = [];

    public void AddMagneticPath(IMagneticPaths magneticPaths)
    {
        _sections = _sections.Append(magneticPaths);
    }

    public Result PassingTheRoute(ITrain train)
    {
        int totalTime = 0;
        foreach (IMagneticPaths path in _sections)
        {
            Result outcome = path.CalculationOfDuration(train);
            if (outcome is Result.SuccessWithTime success)
            {
                totalTime += success.Time;
            }
            else
            {
                return outcome;
            }
        }

        if (train.Speed > MaxSpeedInTheEnd)
        {
            return new Result.ExceedingTheMaxPermissibleSpeed();
        }

        return new Result.SuccessWithTime(totalTime);
    }
}