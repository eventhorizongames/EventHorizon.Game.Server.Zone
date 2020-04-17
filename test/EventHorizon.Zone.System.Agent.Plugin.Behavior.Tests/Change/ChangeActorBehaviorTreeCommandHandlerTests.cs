namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Change
{
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class ChangeActorBehaviorTreeCommandHandlerTests
    {
        [Fact]
        public async Task ShouldIgnoreWhenActorNull()
        {
            //Given
            IObjectEntity inputEntity = null;
            var inputBehaviorTreeId = string.Empty;

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();

            //When
            var handler = new ChangeActorBehaviorTreeCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ChangeActorBehaviorTreeCommand(
                    inputEntity,
                    inputBehaviorTreeId
                ),
                CancellationToken.None
            );

            //Then
            Assert.False(
                actual
            );
        }

        [Fact]
        public async Task ShouldReturnTrueWhenAllIsFine()
        {
            //Given
            var inputEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 1L,
            }.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                AgentBehavior.NEW
            );
            var inputBehaviorTreeId = string.Empty;

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();

            //When
            var handler = new ChangeActorBehaviorTreeCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ChangeActorBehaviorTreeCommand(
                    inputEntity,
                    inputBehaviorTreeId
                ),
                CancellationToken.None
            );

            //Then
            Assert.True(
                actual
            );
        }

        [Fact]
        public async Task ShouldSetNewBehaviorTreeStateWhenActorIsFound()
        {
            //Given
            var inputEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 1L,
            };
            var inputBehaviorTreeId = string.Empty;

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();

            //When
            var handler = new ChangeActorBehaviorTreeCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ChangeActorBehaviorTreeCommand(
                    inputEntity,
                    inputBehaviorTreeId
                ),
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<UpdateEntityCommand>(
                        command => command.Entity.ContainsProperty(
                            BehaviorTreeState.PROPERTY_NAME
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldSetNewAgentBehaviorWhenActorIsFound()
        {
            //Given
            var inputEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 1L,
            };
            var inputBehaviorTreeId = string.Empty;

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();

            //When
            var handler = new ChangeActorBehaviorTreeCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ChangeActorBehaviorTreeCommand(
                    inputEntity,
                    inputBehaviorTreeId
                ),
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<UpdateEntityCommand>(
                        command => command.Entity.ContainsProperty(
                            AgentBehavior.PROPERTY_NAME
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldUpdateEntityWhenActorIsFound()
        {
            //Given
            var inputEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = 1L,
            };
            var inputBehaviorTreeId = string.Empty;

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();

            //When
            var handler = new ChangeActorBehaviorTreeCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ChangeActorBehaviorTreeCommand(
                    inputEntity,
                    inputBehaviorTreeId
                ),
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<UpdateEntityCommand>(
                        command => command.Entity.ContainsProperty(
                            AgentBehavior.PROPERTY_NAME
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldRegisterActorForNextTickCycleWhenActorIsFound()
        {
            //Given
            var expectedActorId = 1L;
            var expectedShapeId = "shape-id";
            var inputEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = expectedActorId,
            };

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var repositoryMock = new Mock<ActorBehaviorTreeRepository>();

            //When
            var handler = new ChangeActorBehaviorTreeCommandHandler(
                mediatorMock.Object,
                dateTimeMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ChangeActorBehaviorTreeCommand(
                    inputEntity,
                    expectedShapeId
                ),
                CancellationToken.None
            );

            //Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RegisterActorWithBehaviorTreeForNextTickCycle>(
                        command => command.ShapeId == expectedShapeId
                            && command.ActorId == expectedActorId
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}