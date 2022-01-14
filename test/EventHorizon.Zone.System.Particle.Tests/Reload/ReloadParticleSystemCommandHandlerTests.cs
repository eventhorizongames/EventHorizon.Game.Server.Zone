namespace EventHorizon.Zone.System.Particle.Tests.Reload;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Particle.Load;
using EventHorizon.Zone.System.Particle.Reload;
using EventHorizon.Zone.System.Particle.State;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ReloadParticleSystemCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldClearAndLoadParticleSystemWhenCommandIsHandled(
        // Given
        [Frozen]
            Mock<IPublisher> publisherMock,
        [Frozen]
            Mock<ParticleTemplateRepository> particleTemplateRepositoryMock,
        ReloadParticleSystemCommandHandler handler
    )
    {
        var expected = new LoadParticleSystemEvent();

        // When
        var actual = await handler.Handle(
            new ReloadParticleSystemCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        particleTemplateRepositoryMock.Verify(
            mock => mock.Clear()
        );
        publisherMock.Verify(
            mock =>
                mock.Publish(
                    expected,
                    CancellationToken.None
                )
        );
    }
}
