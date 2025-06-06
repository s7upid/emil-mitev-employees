using AutoFixture;

namespace UnitTests.Fixtures;

public class RecursionFixture
{
    public static IFixture CreateFixtureWithOmitOnRecursion()
    {
        var fixture = new Fixture();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
