namespace EventHorizon.Zone.Core.Entity.Tests.State
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Entity.Model;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Entity.Tests.TestingModels;
    using EventHorizon.Zone.Core.Events.Json;
    using EventHorizon.Zone.Core.Model.Command;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    public class InMemoryEntitySettingsStateTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldSetEntityConfigurationWhenSetReturnsTrueForUpdated(
            // Given
            ObjectEntityConfigurationTestModel entityConfiguration,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryEntitySettingsState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SerializeToJsonCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    new SerializeToJsonResult(
                        "{}"
                    )
                )
            );

            // When
            var (Updated, OldConfig) = await state.SetConfiguration(
                entityConfiguration,
                CancellationToken.None
            );
            var actual = state.EntityConfiguration;

            // Then
            Updated.Should().BeTrue();
            OldConfig.Should().NotBeNull();

            actual.Should().BeEquivalentTo(
                entityConfiguration
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnNotUpdatedAndOldEntityConfigurationWhenSetIsRanTwiceWithSameConfiguration(
            // Given
            ObjectEntityConfigurationTestModel entityConfiguration,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryEntitySettingsState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    new SerializeToJsonCommand(
                        entityConfiguration
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    new SerializeToJsonResult(
                        "{}"
                    )
                )
            );

            // When
            await state.SetConfiguration(
                entityConfiguration,
                CancellationToken.None
            );
            var (Updated, OldConfig) = await state.SetConfiguration(
                entityConfiguration,
                CancellationToken.None
            );
            var actual = state.EntityConfiguration;

            // Then
            Updated.Should().BeFalse();
            OldConfig.Should().BeEquivalentTo(
                entityConfiguration
            );

            actual.Should().BeEquivalentTo(
                entityConfiguration
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnNotUpdatedConfigurationWhenSetHasIssuesWithSerialization(
            // Given
            string errorCode,
            ObjectEntityConfigurationTestModel entityConfiguration,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryEntitySettingsState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SerializeToJsonCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    errorCode
                )
            );

            // When
            var (Updated, OldConfig) = await state.SetConfiguration(
                entityConfiguration,
                CancellationToken.None
            );
            var actual = state.EntityConfiguration;

            // Then
            Updated.Should().BeFalse();
            OldConfig.Should().NotBeSameAs(
                entityConfiguration
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldSetEntityDataWhenSetReturnsTrueForUpdated(
            // Given
            ObjectEntityDataTestModel entityData,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryEntitySettingsState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SerializeToJsonCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    new SerializeToJsonResult(
                        "{}"
                    )
                )
            );

            // When
            var (Updated, OldData) = await state.SetData(
                entityData,
                CancellationToken.None
            );
            var actual = state.EntityData;

            // Then
            Updated.Should().BeTrue();
            OldData.Should().NotBeNull();

            actual.Should().BeEquivalentTo(
                entityData
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnNotUpdatedAndOldEntityDataWhenSetIsRanTwiceWithSameData(
            // Given
            ObjectEntityDataTestModel entityData,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryEntitySettingsState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    new SerializeToJsonCommand(
                        entityData
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    new SerializeToJsonResult(
                        "{}"
                    )
                )
            );

            // When
            await state.SetData(
                entityData,
                CancellationToken.None
            );
            var (Updated, OldData) = await state.SetData(
                entityData,
                CancellationToken.None
            );
            var actual = state.EntityData;

            // Then
            Updated.Should().BeFalse();
            OldData.Should().BeEquivalentTo(
                entityData
            );

            actual.Should().BeEquivalentTo(
                entityData
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnNotUpdatedDataWhenSetHasIssuesWithSerialization(
            // Given
            string errorCode,
            ObjectEntityDataModel entityData,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryEntitySettingsState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SerializeToJsonCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    errorCode
                )
            );

            // When
            var (Updated, OldConfig) = await state.SetData(
                entityData,
                CancellationToken.None
            );
            var actual = state.EntityData;

            // Then
            Updated.Should().BeFalse();
            OldConfig.Should().NotBeSameAs(
                entityData
            );
        }
    }
}
