using TrainRouteSimulator.General;
using TrainRouteSimulator.SectionOfTheRoute;
using TrainRouteSimulator.Trains;
using Xunit;

namespace TrainRouteSimulator.Tests;

public class UnitTests
{
    [Fact]
    public void ForceConventionalSuccessful()
    {
        var train = new Train(2000, 6000, 100);
        var itinerary = new Itinerary(2000);
        itinerary.AddMagneticPath(new MagneticForcePaths(16000, 4000));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(10000));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.SuccessWithTime(200));
    }

    [Fact]
    public void ForceConventionalUnsuccessful()
    {
        var train = new Train(52, 112, 5);
        var itinerary = new Itinerary(27);
        itinerary.AddMagneticPath(new MagneticForcePaths(999, 100));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(700));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.ExceedingTheMaxPermissibleSpeed);
    }

    [Fact]
    public void ForceTwoConventionalStationSuccessful()
    {
        var train = new Train(85, 124, 5);
        var itinerary = new Itinerary(200);
        itinerary.AddMagneticPath(new MagneticForcePaths(217, 90));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(246));
        itinerary.AddMagneticPath(new Stations(367, 65, 163));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(378));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.SuccessWithTime(140));
    }

    [Fact]
    public void ForceStationConventionalUnsuccessful()
    {
        var train = new Train(100, 600, 10);
        var itinerary = new Itinerary(560);
        itinerary.AddMagneticPath(new MagneticForcePaths(350, 400));
        itinerary.AddMagneticPath(new Stations(840, 140, 35));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(250));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.ExceedingTheSpeedLimit);
    }

    [Fact]
    public void ForceConventionalStationConventionalUnsuccessful()
    {
        var train = new Train(50, 300, 5);
        var itinerary = new Itinerary(50);
        itinerary.AddMagneticPath(new MagneticForcePaths(185, 300));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(257));
        itinerary.AddMagneticPath(new Stations(387, 70, 400));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(333));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.ExceedingTheMaxPermissibleSpeed);
    }

    [Fact]
    public void FourForceThreeConventionalOneStationSuccessful()
    {
        var train = new Train(100, 510, 5);
        var itinerary = new Itinerary(60);
        itinerary.AddMagneticPath(new MagneticForcePaths(500, 500));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(350));
        itinerary.AddMagneticPath(new MagneticForcePaths(600, -300));
        itinerary.AddMagneticPath(new Stations(100, 85, 60));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(400));
        itinerary.AddMagneticPath(new MagneticForcePaths(500, 500));
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(350));
        itinerary.AddMagneticPath(new MagneticForcePaths(600, -300));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.SuccessWithTime(170));
    }

    [Fact]
    public void ConventionalUnsuccessful()
    {
        var train = new Train(200, 200, 5);
        var itinerary = new Itinerary(200);
        itinerary.AddMagneticPath(new ConventionalMagneticPaths(700));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.LackOfSpeedAndAcceleration);
    }

    [Fact]
    public void TwoForceUnsuccessful()
    {
        var train = new Train(50, 75, 5);
        const int x = 300, y = 30;
        var itinerary = new Itinerary(110);
        itinerary.AddMagneticPath(new MagneticForcePaths(x, y));
        itinerary.AddMagneticPath(new MagneticForcePaths(x, y * -2));

        Result outcome = itinerary.PassingTheRoute(train);
        Assert.True(outcome is Result.LackOfSpeedAndAcceleration);
    }
}