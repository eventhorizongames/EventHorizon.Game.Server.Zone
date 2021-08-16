namespace EventHorizon.Zone.System.Agent.Tests.Monitor.Path
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.Path;
    using EventHorizon.Zone.System.Agent.Monitor.Path;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class CheckForStaleAgentPathHandlerTests
    {
        [Fact]
        public async Task ShouldGetExpectedAgentListWhenUsingQueryFromGetAgentListEvent()
        {
            // Given
            GetAgentListEvent getAgentListEvent = default;
            var now = DateTime.Now;

            var agent1 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            LocationState.PROPERTY_NAME,
                            new LocationState
                            {
                                CanMove = true,
                                NextMoveRequest = now.AddSeconds(-20),
                            }
                        },
                        {
                            PathState.PROPERTY_NAME,
                            new PathState()
                                .SetPath(
                                    new Queue<Vector3>(
                                        new List<Vector3>
                                        {
                                            Vector3.Zero
                                        }
                                    )
                                )
                        },
                    }
                )
            );
            agent1.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            );
            agent1.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );
            var agent2 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            LocationState.PROPERTY_NAME,
                            new LocationState
                            {
                                CanMove = true
                            }
                        },
                        {
                            PathState.PROPERTY_NAME,
                            new PathState()
                                .SetPath(
                                    new Queue<Vector3>(
                                        new List<Vector3>
                                        {
                                            Vector3.Zero
                                        }
                                    )
                                )
                        },
                    }
                )
            );
            agent2.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            );
            agent2.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );
            var agent3 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            LocationState.PROPERTY_NAME,
                            new LocationState
                            {
                                CanMove = true
                            }
                        },
                        {
                            PathState.PROPERTY_NAME,
                            new PathState()
                                .SetPath(
                                    new Queue<Vector3>(
                                        new List<Vector3>
                                        {
                                            Vector3.Zero
                                        }
                                    )
                                )
                        },
                    }
                )
            );
            agent3.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            );
            agent3.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );
            var agent4 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            LocationState.PROPERTY_NAME,
                            new LocationState
                            {
                                CanMove = false
                            }
                        }
                    }
                )
            );
            agent4.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            );
            var agent5 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            LocationState.PROPERTY_NAME,
                            new LocationState
                            {
                                CanMove = true
                            }
                        },
                        {
                            PathState.PROPERTY_NAME,
                            new PathState()
                                .SetPath(
                                    new Queue<Vector3>()
                                )
                        },
                    }
                )
            );
            agent5.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            );
            agent5.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );
            var agent6 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            LocationState.PROPERTY_NAME,
                            new LocationState
                            {
                                CanMove = true,
                                NextMoveRequest = now.AddSeconds(10),
                            }
                        },
                        {
                            PathState.PROPERTY_NAME,
                            new PathState()
                                .SetPath(
                                    new Queue<Vector3>(
                                        new List<Vector3>{
                                            Vector3.Zero,
                                        }
                                    )
                                )
                        },
                    }
                )
            );
            agent6.PopulateData<LocationState>(
                LocationState.PROPERTY_NAME
            );
            agent6.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );

            var agentRepository = new List<AgentEntity>
            {
                agent1,
                agent2,
                agent3,
                agent4,
                agent5,
                agent6,
            };

            var expected = new List<AgentEntity>
            {
                agent1,
                agent2,
                agent3,
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeServiceMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<GetAgentListEvent>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<IEnumerable<AgentEntity>>, CancellationToken>(
                (resultEvent, token) => getAgentListEvent = (GetAgentListEvent)resultEvent
            );

            dateTimeServiceMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new CheckForStaleAgentPathHandler(
                mediatorMock.Object,
                dateTimeServiceMock.Object
            );
            await handler.Handle(
                new CheckForStaleAgentPath(),
                CancellationToken.None
            );

            var actual = agentRepository.Where(
                getAgentListEvent.Query
            );

            // Then
            actual.Should()
                .BeEquivalentTo(
                    expected
                );
        }

        [Fact]
        public async Task ShouldQueueAgentToMoveWhenAgentListReturnedFromQuery()
        {
            // Given
            var agent1Id = 101L;
            var agent1MoveTo = new Vector3(2, 2, 1);
            var agent1Path = new Queue<Vector3>(
                new List<Vector3>
                {
                    Vector3.One
                }
            );
            var agent1 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            PathState.PROPERTY_NAME,
                            new PathState
                            {
                                MoveTo = agent1MoveTo,
                            }.SetPath(
                                agent1Path
                            )
                        },
                    }
                )
            )
            {
                Id = agent1Id
            };
            agent1.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );
            var expectedQueueAgentToMove1 = new QueueAgentToMove(
                agent1Id,
                agent1Path,
                agent1MoveTo
            );
            var agent2Id = 102L;
            var agent2MoveTo = new Vector3(1, 2, 3);
            var agent2Path = new Queue<Vector3>(
                new List<Vector3>
                {
                    Vector3.Zero
                }
            );
            var agent2 = new AgentEntity(
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        {
                            PathState.PROPERTY_NAME,
                            new PathState
                            {
                                MoveTo = agent2MoveTo,
                            }.SetPath(
                                agent2Path
                            )
                        },
                    }
                )
            )
            {
                Id = agent2Id
            };
            agent2.PopulateData<PathState>(
                PathState.PROPERTY_NAME
            );
            var expectedQueueAgentToMove2 = new QueueAgentToMove(
                agent2Id,
                agent2Path,
                agent2MoveTo
            );

            var agentList = new List<AgentEntity>
            {
                agent1,
                agent2,
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeServiceMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<GetAgentListEvent>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentList
            );

            // When
            var handler = new CheckForStaleAgentPathHandler(
                mediatorMock.Object,
                dateTimeServiceMock.Object
            );
            await handler.Handle(
                new CheckForStaleAgentPath(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedQueueAgentToMove1,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedQueueAgentToMove2,
                    CancellationToken.None
                )
            );
        }
    }

}
