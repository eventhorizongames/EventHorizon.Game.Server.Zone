namespace EventHorizon.Test.Common.Attributes
{
    using System;

    using AutoFixture;
    using AutoFixture.AutoMoq;

    using EventHorizon.Test.Common.Utils;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    [AttributeUsage(AttributeTargets.Method)]
    public class AutoServiceProviderDataAttribute
        : AutoMoqDataAttribute
    {
        public AutoServiceProviderDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(
                new AutoMoqCustomization
                {
                    ConfigureMembers = true,
                    GenerateDelegates = true,
                }
            );

            fixture.Freeze<Mock<IServiceScope>>();
            fixture.Freeze<Mock<IServiceProvider>>();
            fixture.Freeze<ServiceScopeFactoryMock>();

            return fixture;
        }
    }
}
