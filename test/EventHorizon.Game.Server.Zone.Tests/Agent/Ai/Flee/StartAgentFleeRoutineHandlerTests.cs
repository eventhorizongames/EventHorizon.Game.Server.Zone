using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using MediatR;
using EventHorizon.Game.Server.Zone.Core.RandomNumber;
using EventHorizon.Game.Server.Zone.Agent.Ai.Flee.Handler;
using EventHorizon.Game.Server.Zone.Agent.Ai.Flee;
using EventHorizon.Game.Server.Zone.ServerAction.Add;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Path.Find;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Models;
using EventHorizon.Game.Server.Zone.Entity.Search;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Find;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Ai.Flee
{
    public class StartAgentFleeRoutineHandlerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public StartAgentFleeRoutineHandlerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestHandle_ShouldNotProcessWhenAgentNotFound()
        {
            // Given
            var inputId = 123;
            var inputStartAgentFleeRoutineEvent = new StartAgentFleeRoutineEvent
            {
                AgentId = inputId
            };

            var loggerMock = new Mock<ILogger<StartAgentFleeRoutineHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            // When
            var startAgentFleeRoutineHandler = new StartAgentFleeRoutineHandler(
                loggerMock.Object,
                mediatorMock.Object,
                randomNumberGeneratorMock.Object
            );

            await startAgentFleeRoutineHandler.Handle(inputStartAgentFleeRoutineEvent, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(It.IsAny<AddServerActionEvent>(), CancellationToken.None), Times.Never());
        }

        [Fact]
        public async Task TestHandle_ShouldCheckFleeInFutureWhenRoutineIsFleeingAndPathIsGreaterThanTwo()
        {
            // Given
            var inputId = 123;
            var inputStartAgentFleeRoutineEvent = new StartAgentFleeRoutineEvent
            {
                AgentId = inputId
            };

            var agentPath = new Queue<Vector3>();
            agentPath.Enqueue(new Vector3(2));
            agentPath.Enqueue(new Vector3(3));
            agentPath.Enqueue(new Vector3(4));
            dynamic agentData = new Dictionary<string, object>{
                {
                    "Routine",
                    AiRoutine.FLEEING
                }
            };
            var agent = new AgentEntity
            {
                Path = agentPath,
                Data = agentData,
            };
            agent.PopulateFromTempData<AiRoutine>("Routine");

            var loggerMock = new Mock<ILogger<StartAgentFleeRoutineHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            mediatorMock.Setup(a => a.Send(new GetAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None)).ReturnsAsync(agent);

            // When
            var startAgentFleeRoutineHandler = new StartAgentFleeRoutineHandler(
                loggerMock.Object,
                mediatorMock.Object,
                randomNumberGeneratorMock.Object
            );

            await startAgentFleeRoutineHandler.Handle(inputStartAgentFleeRoutineEvent, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(It.IsAny<AddServerActionEvent>(), CancellationToken.None));
        }

        [Fact]
        public async Task TestHandle_ShouldPublishStartAgentRoutineEventWhenRoutineIsFleeAndNoEntitesInSight()
        {
            // Given
            var inputId = 123;
            var inputStartAgentFleeRoutineEvent = new StartAgentFleeRoutineEvent
            {
                AgentId = inputId
            };
            var fallbackRoutine = AiRoutine.IDLE;

            var agent = new AgentEntity
            {
                Id = inputId,
                Data = new Dictionary<string, object>{
                    {
                        "Routine",
                        AiRoutine.FLEE
                    },
                    {
                        "Ai",
                        new AgentAiState
                        {
                            Flee = new AgentFleeState
                            {
                                FallbackRoutine = fallbackRoutine
                            }
                        }
                    }
                }
            };
            agent.PopulateFromTempData<AiRoutine>("Routine");
            agent.PopulateFromTempData<AgentAiState>("Ai");

            var expectedStartRoutineEvent = new StartAgentRoutineEvent
            {
                AgentId = inputId,
                Routine = fallbackRoutine
            };

            var loggerMock = new Mock<ILogger<StartAgentFleeRoutineHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            mediatorMock.Setup(a => a.Send(new GetAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None)).ReturnsAsync(agent);

            // When
            var startAgentFleeRoutineHandler = new StartAgentFleeRoutineHandler(
                loggerMock.Object,
                mediatorMock.Object,
                randomNumberGeneratorMock.Object
            );

            await startAgentFleeRoutineHandler.Handle(inputStartAgentFleeRoutineEvent, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(expectedStartRoutineEvent, CancellationToken.None));
        }

        [Fact]
        public async Task TestHandle_ShouldPublishStartAgentRoutineEventWhenNullPathToFleeToIsReturned()
        {
            // Given
            var inputId = 123;
            var inputStartAgentFleeRoutineEvent = new StartAgentFleeRoutineEvent
            {
                AgentId = inputId
            };

            var currentPosition = new Vector3(3, 0, 0);
            var entityInSightCurrentPosition = new Vector3(4, 0, 0);
            var distanceToRun = 10;
            var pathLookupToPosition = new Vector3(-7, 0, 0);
            var sightDistance = 10;
            var tagList = new List<string> { "any" };
            var fallbackRoutine = AiRoutine.IDLE;

            var agent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    CurrentPosition = currentPosition
                },
                Data = new Dictionary<string, object>{
                    {
                        "Routine",
                        AiRoutine.FLEE
                    },
                    {
                        "Ai",
                        new AgentAiState
                        {
                            Flee = new AgentFleeState
                            {
                                FallbackRoutine = fallbackRoutine,
                                DistanceToRun = distanceToRun,
                                SightDistance = sightDistance,
                                TagList = tagList,
                            }
                        }
                    }
                }
            };
            agent.PopulateFromTempData<AiRoutine>("Routine");
            agent.PopulateFromTempData<AgentAiState>("Ai");
            Queue<Vector3> path = null;

            var entityInSightId = 321;
            var entityInSight = new TestObjectEntity
            {
                Id = entityInSightId,
                Position = new PositionState
                {
                    CurrentPosition = entityInSightCurrentPosition
                }
            };
            var entityInSightList = new List<long>
            {
                entityInSightId
            };

            var expectedStartRoutineEvent = new StartAgentRoutineEvent
            {
                AgentId = inputId,
                Routine = fallbackRoutine
            };
            var expectedFindPathEvent = new FindPathEvent
            {
                From = currentPosition,
                To = pathLookupToPosition
            };

            var loggerMock = new Mock<ILogger<StartAgentFleeRoutineHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            mediatorMock.Setup(a => a.Send(new GetAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None)).ReturnsAsync(agent);
            mediatorMock.Setup(a => a.Send(new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = currentPosition,
                SearchRadius = sightDistance,
                TagList = tagList
            }, CancellationToken.None)).ReturnsAsync(entityInSightList);
            mediatorMock.Setup(a => a.Send(new GetEntityByIdEvent
            {
                EntityId = entityInSightId
            }, CancellationToken.None)).ReturnsAsync(entityInSight);
            mediatorMock.Setup(a => a.Send(expectedFindPathEvent, CancellationToken.None))
                .ReturnsAsync(path);

            // When
            var startAgentFleeRoutineHandler = new StartAgentFleeRoutineHandler(
                loggerMock.Object,
                mediatorMock.Object,
                randomNumberGeneratorMock.Object
            );

            await startAgentFleeRoutineHandler.Handle(inputStartAgentFleeRoutineEvent, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(expectedStartRoutineEvent, CancellationToken.None));
            mediatorMock.Verify(a => a.Send(expectedFindPathEvent, CancellationToken.None));
        }

        [Fact]
        public async Task TestHandle_ShouldSetAgentsRoutineToFleeAndRegisterAgentMovePath()
        {
            // Given
            var inputId = 123;
            var inputEntityInSightCurrentPosition = new Vector3(4, 0, 0);
            var inputStartAgentFleeRoutineEvent = new StartAgentFleeRoutineEvent
            {
                AgentId = inputId
            };
            var testData = this.SetupTestData(inputId, inputEntityInSightCurrentPosition);

            // Injections
            var loggerMock = new Mock<ILogger<StartAgentFleeRoutineHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            mediatorMock.Setup(a => a.Send(new GetAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None)).ReturnsAsync(testData.Agent);
            mediatorMock.Setup(a => a.Send(new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = testData.CurrentPosition,
                SearchRadius = testData.SightDistance,
                TagList = testData.TagList
            }, CancellationToken.None)).ReturnsAsync(testData.EntityInSightList);
            mediatorMock.Setup(a => a.Send(new GetEntityByIdEvent
            {
                EntityId = testData.EntityInSightId
            }, CancellationToken.None)).ReturnsAsync(testData.EntityInSight);
            mediatorMock.Setup(a => a.Send(new FindPathEvent
            {
                From = testData.CurrentPosition,
                To = testData.PathLookupToPosition
            }, CancellationToken.None))
                .ReturnsAsync(testData.Path);

            // Expected
            var expectedFindPathEvent = new FindPathEvent
            {
                From = testData.CurrentPosition,
                To = testData.PathLookupToPosition
            };
            var expectedSetAgentRoutineEvent = new SetAgentRoutineEvent
            {
                AgentId = inputId,
                Routine = AiRoutine.FLEEING
            };
            var expectedRegisterAgentMovePathEvent = new RegisterAgentMovePathEvent
            {
                AgentId = inputId,
                Path = testData.Path
            };

            // When
            var startAgentFleeRoutineHandler = new StartAgentFleeRoutineHandler(
                loggerMock.Object,
                mediatorMock.Object,
                randomNumberGeneratorMock.Object
            );

            await startAgentFleeRoutineHandler.Handle(inputStartAgentFleeRoutineEvent, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Publish(It.IsAny<StartAgentRoutineEvent>(), CancellationToken.None), Times.Never());
            mediatorMock.Verify(a => a.Send(expectedFindPathEvent, CancellationToken.None));
            mediatorMock.Verify(a => a.Publish(expectedSetAgentRoutineEvent, CancellationToken.None));
            mediatorMock.Verify(a => a.Publish(expectedRegisterAgentMovePathEvent, CancellationToken.None));
            mediatorMock.Verify(a => a.Publish(It.IsAny<AddServerActionEvent>(), CancellationToken.None));
        }

        [Theory]
        [InlineData(1, 13, 0)]
        [InlineData(2, 3, 10)]
        [InlineData(3, -7, 0)]
        [InlineData(4, 3, -10)]
        public async Task TestHandle_ShouldUseRandomNumberGeneratorToPickRandomDistnaceToFleeWhenInvalidFleeDirection(int inputDistanceResponse, float expectedToPositionX, float expectedToPositionZ)
        {
            // Given
            var inputId = 123;
            var inputEntityInSightCurrentPosition = new Vector3(float.NaN);
            var inputStartAgentFleeRoutineEvent = new StartAgentFleeRoutineEvent
            {
                AgentId = inputId
            };
            var testData = this.SetupTestData(inputId, inputEntityInSightCurrentPosition);


            var expectedToPosition = new Vector3(expectedToPositionX, 0, expectedToPositionZ);
            // Expected
            var expectedFindPathEvent = new FindPathEvent
            {
                From = testData.CurrentPosition,
                To = expectedToPosition
            };

            // Injections
            var loggerMock = new Mock<ILogger<StartAgentFleeRoutineHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();

            mediatorMock.Setup(a => a.Send(new GetAgentEvent
            {
                AgentId = inputId
            }, CancellationToken.None)).ReturnsAsync(testData.Agent);
            mediatorMock.Setup(a => a.Send(new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = testData.CurrentPosition,
                SearchRadius = testData.SightDistance,
                TagList = testData.TagList
            }, CancellationToken.None)).ReturnsAsync(testData.EntityInSightList);
            mediatorMock.Setup(a => a.Send(new GetEntityByIdEvent
            {
                EntityId = testData.EntityInSightId
            }, CancellationToken.None)).ReturnsAsync(testData.EntityInSight);
            mediatorMock.Setup(a => a.Send(new FindPathEvent
            {
                From = testData.CurrentPosition,
                To = testData.PathLookupToPosition
            }, CancellationToken.None))
                .ReturnsAsync(testData.Path);
            randomNumberGeneratorMock.Setup(a => a.Next(1, 4)).Returns(inputDistanceResponse);

            // When
            var startAgentFleeRoutineHandler = new StartAgentFleeRoutineHandler(
                loggerMock.Object,
                mediatorMock.Object,
                randomNumberGeneratorMock.Object
            );

            await startAgentFleeRoutineHandler.Handle(inputStartAgentFleeRoutineEvent, CancellationToken.None);

            // Then
            mediatorMock.Verify(a => a.Send(expectedFindPathEvent, CancellationToken.None));
        }

        private TestData SetupTestData(int inputId, Vector3 entityInSightCurrentPosition)
        {
            var currentPosition = new Vector3(3, 0, 0);
            var distanceToRun = 10;
            var pathLookupToPosition = new Vector3(-7, 0, 0);
            var sightDistance = 10;
            var tagList = new List<string> { "any" };

            var agent = new AgentEntity
            {
                Id = inputId,
                Position = new PositionState
                {
                    CurrentPosition = currentPosition
                },
                Data = new Dictionary<string, object>{
                    {
                        "Routine",
                        AiRoutine.FLEE
                    },
                    {
                        "Ai",
                        new AgentAiState
                        {
                            Flee = new AgentFleeState
                            {
                                DistanceToRun = distanceToRun,
                                SightDistance = sightDistance,
                                TagList = tagList,
                            }
                        }
                    }
                }
            };
            agent.PopulateFromTempData<AiRoutine>("Routine");
            agent.PopulateFromTempData<AgentAiState>("Ai");
            Queue<Vector3> path = new Queue<Vector3>();
            path.Enqueue(new Vector3(2));

            var entityInSightId = 321;
            var entityInSight = new TestObjectEntity
            {
                Id = entityInSightId,
                Position = new PositionState
                {
                    CurrentPosition = entityInSightCurrentPosition
                }
            };
            var entityInSightList = new List<long>
            {
                entityInSightId
            };

            return new TestData
            {
                Agent = agent,
                CurrentPosition = currentPosition,
                SightDistance = sightDistance,
                TagList = tagList,
                EntityInSightList = entityInSightList,
                EntityInSightId = entityInSightId,
                EntityInSight = entityInSight,
                PathLookupToPosition = pathLookupToPosition,
                Path = path,
            };
        }

        private struct TestData
        {
            public AgentEntity Agent { get; set; }
            public Vector3 CurrentPosition { get; set; }
            public int SightDistance { get; set; }
            public IList<string> TagList { get; set; }
            public IList<long> EntityInSightList { get; set; }
            public long EntityInSightId { get; set; }
            public IObjectEntity EntityInSight { get; set; }
            public Vector3 PathLookupToPosition { get; set; }
            public Queue<Vector3> Path { get; set; }
        }
    }
}