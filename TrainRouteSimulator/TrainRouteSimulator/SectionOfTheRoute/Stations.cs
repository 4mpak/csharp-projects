using TrainRouteSimulator.General;
using TrainRouteSimulator.SectionOfTheRoute.Interfaces;
using TrainRouteSimulator.Trains;

namespace TrainRouteSimulator.SectionOfTheRoute;

public class Stations(int distance, int peopleOccupancy, int speedLimit) : IMagneticPaths
{
    private int PeopleOccupancy { get; } = peopleOccupancy;

    private int Distance { get; } = distance;

    private int SpeedLimit { get; } = speedLimit;

    public Result CalculationOfDuration(ITrain train)
    {
        train.Acceleration = 0;
        if (train.Speed > SpeedLimit)
        {
            return new Result.ExceedingTheSpeedLimit();
        }

        Result outcome = train.CalculationOfDuration(Distance);
        if (outcome is Result.SuccessWithTime success)
        {
            return new Result.SuccessWithTime(success.Time + PeopleOccupancy);
        }

        return outcome;
    }
}