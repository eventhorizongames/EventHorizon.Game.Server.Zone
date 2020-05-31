namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
    using FluentAssertions;
    using global::System.Threading;
    using MediatR;
    using Moq;
    using Xunit;

    public class SystemCompbatPluginSkillExtensionsTests
    {
        [Fact]
        public void TestAddEntity_ShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemCombatPluginSkillExtensions.AddSystemCombatPluginSkill(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(SkillRepository), service.ServiceType);
                    Assert.Equal(typeof(InMemorySkillRepository), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void ShouldReturnBuilderWhenCalledWithBuilder()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemCombatPluginSkillExtensions.UseSystemCombatPluginSkill(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldPublishLoadSystemCombatPluginSkillWhenUseExtensionIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new LoadSystemCombatPluginSkill();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemCombatPluginSkillExtensions.UseSystemCombatPluginSkill(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
