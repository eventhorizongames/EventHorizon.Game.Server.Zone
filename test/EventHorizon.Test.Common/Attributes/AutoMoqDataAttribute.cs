namespace EventHorizon.Test.Common.Attributes;

using System;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

[AttributeUsage(AttributeTargets.Method)]
public class AutoMoqDataAttribute
    : AutoDataAttribute
{
    public AutoMoqDataAttribute() 
        : base(() => CreateFixture()) { }

    public AutoMoqDataAttribute(
        bool disableRecursionCheck
    ) : base(() => CreateFixture(disableRecursionCheck))
    {
    }

    public AutoMoqDataAttribute(
        Func<IFixture> fixtureFactory
    ) : base(fixtureFactory) { }

    private static IFixture CreateFixture(
        bool disableRecursionCheck = false
    )
    {
        var fixture = new Fixture();

        if (disableRecursionCheck)
        {
            fixture.Behaviors.Remove(
                new ThrowingRecursionBehavior()
            );
            fixture.Behaviors.Add(
                new OmitOnRecursionBehavior()
            );
        }

        fixture.Customize(
            new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true,
            }
        );

        return fixture;
    }
}
