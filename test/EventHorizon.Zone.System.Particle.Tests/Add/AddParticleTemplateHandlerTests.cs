namespace EventHorizon.Zone.System.Particle.Tests.Add;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Particle.Add;
using EventHorizon.Zone.System.Particle.Events.Add;
using EventHorizon.Zone.System.Particle.Model.Template;
using EventHorizon.Zone.System.Particle.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class AddParticleTemplateHandlerTests
{
    [Theory, AutoMoqData]
    public async Task TestShouldAddTemplateToRepositoryWhenHandleIsCalled(
        // Given
        string templateId,
        [Frozen]
            Mock<ParticleTemplateRepository> templateRepositoryMock,
        AddParticleTemplateHandler handler
    )
    {
        var particleTemplate = new ParticleTemplate();

        // When
        await handler.Handle(
            new AddParticleTemplateEvent(
                templateId,
                particleTemplate
            ),
            CancellationToken.None
        );

        // Then
        templateRepositoryMock.Verify(
            mock => mock.Add(templateId, particleTemplate)
        );
    }
}
