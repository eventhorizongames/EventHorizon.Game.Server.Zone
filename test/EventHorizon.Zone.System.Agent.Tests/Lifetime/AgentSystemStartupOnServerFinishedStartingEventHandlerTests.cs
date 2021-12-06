namespace EventHorizon.Zone.System.Agent.Tests.Lifetime;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.Settings;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Lifetime;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Save.Model;

using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class AgentSystemStartupOnServerFinishedStartingEventHandlerTests
{
    [Theory, AutoMoqData]
    public async Task CorrectlyRegistersAgentsWhenFileLoaderReturnsAgentList(
        // Given
        string appDataPath,
        [Frozen] ZoneSettings zoneSettings,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<IAgentConnection> agentConnectionMock,
        [Frozen] Mock<IJsonFileLoader> fileLoaderMock,
        AgentSystemStartupOnServerFinishedStartingEventHandler handler
    )
    {
        var expectedAgentFileName = Path.Combine(
            appDataPath,
            "Agent",
            "Agent.state.json"
        );
        var expectedTag = "server-tag";
        var expectedAgentDetails1 = new AgentDetails
        {
            Id = Guid.NewGuid().ToString(),
            Transform = new TransformState(),
        };
        var expectedAgentDetails2 = new AgentDetails
        {
            Transform = new TransformState(),
        };
        var expectedAgentList = new List<AgentDetails>()
        {
            expectedAgentDetails1,
            expectedAgentDetails2,
        };
        var expectedAgentState = new AgentSaveState
        {
            AgentList = expectedAgentList,
        };

        zoneSettings.Tag = expectedTag;

        agentConnectionMock.Setup(
            mock => mock.SendAction<IList<AgentDetails>>(
                "GetAgentsByZoneTag",
                expectedTag
            )
        ).ReturnsAsync(
            expectedAgentList
        );

        serverInfoMock.Setup(
            mock => mock.AppDataPath
        ).Returns(
            appDataPath
        );

        fileLoaderMock.Setup(
            mock => mock.GetFile<AgentSaveState>(
                expectedAgentFileName
            )
        ).ReturnsAsync(
            expectedAgentState
        );

        // When
        await handler.Handle(
            new ServerFinishedStartingEvent(),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                It.IsAny<RegisterAgentEvent>(),
                CancellationToken.None
            ),
            Times.Exactly(
                4
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task SuccessfullyHandledEventWhenFileLoaderReturnsDefaultAgentSaveState(
        // Given
        [Frozen] Mock<IJsonFileLoader> fileLoaderMock,
        [Frozen] Mock<ISender> senderMock,
        AgentSystemStartupOnServerFinishedStartingEventHandler handler
    )
    {
        fileLoaderMock.Setup(
            mock => mock.GetFile<AgentSaveState>(
                It.IsAny<string>()
            )
        ).ReturnsAsync(
            default(AgentSaveState)
        );

        // When
        await handler.Handle(
            new ServerFinishedStartingEvent(),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                It.IsAny<RegisterAgentEvent>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }

    [Theory, AutoMoqData]
    public async Task UnRegisterAgentsWhenPlatformContainsExistingAgents(
        // Given
        [Frozen] Mock<ISender> senderMock,
        AgentSystemStartupOnServerFinishedStartingEventHandler handler
    )
    {
        var agentList = new List<AgentEntity>
        {
            new AgentEntity(),
        };
        senderMock.Setup(
            mock => mock.Send(
                new GetAgentListEvent(null),
                CancellationToken.None
            )
        ).ReturnsAsync(
            agentList
        );

        // When
        await handler.Handle(
            new ServerFinishedStartingEvent(),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                It.IsAny<UnRegisterAgent>(),
                CancellationToken.None
            )
        );
    }
}
