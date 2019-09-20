using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Performance;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Update;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Behavior.Update.RunBehaviorTreeUpdate;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.Update
{
    public class RunBehaviorTreeUpdateHandlerTests
    {
        [Fact]
        public async Task ShouldLoadInAnyActiveBehaviorTreesAndCallUpdate()
        {
            // Given
            var treeId = "tree-id";
            var runBehaviorTreeUpdate = new RunBehaviorTreeUpdate(
                treeId
            );
            var actorId1 = 1;
            var actorId2 = 2;
            var actorIdList = new List<long>()
            {
                actorId1,
                actorId2
            };
            var expectedActor1 = new DefaultEntity
            {
                Id = actorId1
            };
            var expectedActor2 = new DefaultEntity
            {
                Id = actorId2
            };
            var expectedBehaviorTreeShap = new ActorBehaviorTreeShape();

            var mediatorMock = new Mock<IMediator>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var behaviorInterpreterKernelMock = new Mock<BehaviorInterpreterKernel>();
            var performanceTrackerMock = new Mock<IPerformanceTracker>();

            mediatorMock.Setup(
                mediator => mediator.Send(
                    new GetEntityByIdEvent
                    {
                        EntityId = actorId1
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedActor1
            );
            mediatorMock.Setup(
                mediator => mediator.Send(
                    new GetEntityByIdEvent
                    {
                        EntityId = actorId2
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedActor2
            );

            actorBehaviorTreeRepositoryMock.Setup(
                repository => repository.ActorIdList(
                    treeId
                )
            ).Returns(
                actorIdList
            );

            actorBehaviorTreeRepositoryMock.Setup(
                repository => repository.FindTreeShape(
                    treeId
                )
            ).Returns(
                expectedBehaviorTreeShap
            );

            // When
            var runBehaviorTreeUpdateHandler = new RunBehaviorTreeUpdateHandler(
                mediatorMock.Object,
                actorBehaviorTreeRepositoryMock.Object,
                behaviorInterpreterKernelMock.Object
            );
            await runBehaviorTreeUpdateHandler.Handle(
                runBehaviorTreeUpdate,
                CancellationToken.None
            );

            // Then
            behaviorInterpreterKernelMock.Verify(
                kernel => kernel.Tick(
                    expectedBehaviorTreeShap,
                    expectedActor1
                )
            );
            behaviorInterpreterKernelMock.Verify(
                kernel => kernel.Tick(
                    expectedBehaviorTreeShap,
                    expectedActor2
                )
            );
        }
    }
}