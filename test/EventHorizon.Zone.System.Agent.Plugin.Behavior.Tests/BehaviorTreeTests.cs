namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests
{
    using global::System;
    using global::System.Diagnostics;
    using global::System.IO;
    using global::System.Text;
    using global::System.Threading.Tasks;
    using global::System.Threading;
    using EventHorizon.Zone.Core.Model.Entity;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
    using Moq;
    using MediatR;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
    using EventHorizon.Tests.TestUtils;
    using EventHorizon.Zone.Core.Reporter.Model;

    public class BehaviorTreeTests : TestFixtureBase
    {
        public BehaviorTreeTests(
            ITestOutputHelper testOutputHelper
        ) : base(
            testOutputHelper
        )
        { }

        private Mock<IServiceScopeFactory> SetupServiceScopeFactoryWithMediatorMock(
            Mock<IMediator> mediatorMock
        )
        {
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceScopeFactoryMock.Setup(
                scopeFactory => scopeFactory.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceProviderMock.Setup(
                serviceProvider => serviceProvider.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            return serviceScopeFactoryMock;
        }

        [Fact]
        public async Task AllNodesShouldHaveSuccessStatus()
        {
            // Given
            var expected = "SUCCESS";
            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    Path.Combine(
                        "Data",
                        "AllNodesShouldHaveSuccessStatus.json"
                    )
                )
            );

            var treeShape = new ActorBehaviorTreeShape(
                "AllNodesShouldHaveSuccessStatus.json",
                agentBehaviorTree
            );
            var actor = new DefaultEntity(
                null
            );

            var actionInterpreterLoggerMock = new Mock<ILogger<ActionInterpreter>>();
            var conditionInterpreterLoggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );
            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                )
            );

            var interpreter = new BehaviorInterpreterDoWhileKernel(
                new BehaviorInterpreterInMemoryMap(
                    new ActionInterpreter(
                        actionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    ),
                    new ConditionInterpreter(
                        conditionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                ),
                new Mock<ReportTracker>().Object
            );

            // When
            var actual = await interpreter.Tick(
                treeShape,
                actor
            );

            // Then
            Assert.Collection(
                actual.NodeList(),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected)
            );
        }
        [Fact]
        public async Task ShouldBeAbleToReuseActorStateWhenEntityContainsExistingState()
        {
            // Given
            var expected = "SUCCESS";
            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    Path.Combine(
                        "Data",
                        "AllNodesShouldHaveSuccessStatus.json"
                    )
                )
            );

            var treeShape = new ActorBehaviorTreeShape(
                "AllNodesShouldHaveSuccessStatus.json",
                agentBehaviorTree
            );
            var actor = new DefaultEntity(
                null
            );
            actor.SetProperty(
                BehaviorTreeState.PROPERTY_NAME,
                new BehaviorTreeState(
                    treeShape
                )
            );

            var actionInterpreterLoggerMock = new Mock<ILogger<ActionInterpreter>>();
            var conditionInterpreterLoggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );
            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                )
            );

            // To be tested
            var interpreter = new BehaviorInterpreterDoWhileKernel(
                new BehaviorInterpreterInMemoryMap(
                    new ActionInterpreter(
                        actionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    ),
                    new ConditionInterpreter(
                        conditionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                ),
                new Mock<ReportTracker>().Object
            );

            // When
            var actual = await interpreter.Tick(
                treeShape,
                actor
            );

            // Then
            Assert.Collection(
                actual.NodeList(),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected),
                node => Assert.Equal(node.Status, expected)
            );
        }

        [Fact]
        public async Task ShouldRunScriptCommandsForActionNode()
        {
            // Given
            var expectedStatus = "SUCCESS";
            var expectedScriptId1 = "Behavior_Action_FindNewMoveLocation";
            var expectedScriptId2 = "Behavior_Action_MoveToLocation";

            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    Path.Combine(
                        "Data",
                        "ShouldRunScriptCommandsForActionNode.json"
                    )
                )
            );
            var treeShape = new ActorBehaviorTreeShape(
                "ShouldRunScriptCommandsForActionNode.json",
                agentBehaviorTree
            );

            var actor = new DefaultEntity(
                null
            );

            var actionInterpreterLoggerMock = new Mock<ILogger<ActionInterpreter>>();
            var conditionInterpreterLoggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );
            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                )
            );
            var interpreter = new BehaviorInterpreterDoWhileKernel(
                new BehaviorInterpreterInMemoryMap(
                    new ActionInterpreter(
                        actionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    ),
                    new ConditionInterpreter(
                        conditionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                ),
                new Mock<ReportTracker>().Object
            );

            // When
            var actual = await interpreter.Tick(
                treeShape,
                actor
            );

            // Then
            Assert.Collection(
                actual.NodeList(),
                node => Assert.Equal(node.Status, expectedStatus),
                node => Assert.Equal(node.Status, expectedStatus),
                node => Assert.Equal(node.Status, expectedStatus)
            );
            Assert.Equal(
                new RunBehaviorScript(
                    actor,
                    expectedScriptId2
                ),
                new RunBehaviorScript(
                    actor,
                    expectedScriptId2
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new RunBehaviorScript(
                        actor,
                        expectedScriptId1
                    ),
                    It.IsAny<CancellationToken>()
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new RunBehaviorScript(
                        actor,
                        expectedScriptId2
                    ),
                    It.IsAny<CancellationToken>()
                )
            );
        }

        [Fact]
        public async Task TestPlayground()
        {
            // Create a Tree
            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    Path.Combine(
                        "Data",
                        "FollowPlayer.json"
                    )
                )
            );
            /* Tree Shape
            The Tree Shape is an represtation of the BT,
                this is static and helps with the navigation of the tree
                by the Interpreter. */
            var treeShape = new ActorBehaviorTreeShape(
                "FollowPlayer.json",
                agentBehaviorTree
            );

            /*  Actor BT Data
            This is the Data for a specific instance.
            Saved between Interpreter Ticks for specific Actor. */
            var actor = new DefaultEntity(
                null
            );



            /* Interpreter 
            Reads in the BT Shape and Actor BT Data. */
            // Setup Infrastructure mock
            var actionInterpreterLoggerMock = new Mock<ILogger<ActionInterpreter>>();
            var conditionInterpreterLoggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );
            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(
                // Any action just return SUCCESS Status
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                )
            );

            // To be tested
            var interpreter = new BehaviorInterpreterDoWhileKernel(
                new BehaviorInterpreterInMemoryMap(
                    new ActionInterpreter(
                        actionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    ),
                    new ConditionInterpreter(
                        conditionInterpreterLoggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                ),
                new Mock<ReportTracker>().Object
            );

            var state = await interpreter.Tick(
                treeShape,
                actor
            );
            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                state = await interpreter.Tick(
                    treeShape,
                    actor
                );
                actor.SetProperty(
                    BehaviorTreeState.PROPERTY_NAME,
                    state
                );
            }

            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine("Time: {0}ms", elapsed);


            await File.WriteAllBytesAsync(
                Path.Combine(
                    "Data",
                    "StateSaved.json"
                ),
                JsonConvert.SerializeObject(
                    state
                ).ToBytes()
            );

            /* Interpreter Data 
            This is internal data that is saved from traversing 
                the action/decider of the BT.
            Only saved for tick of Interpreter. */

            /* Deferred Actions 
            These are actions that should be run at the end of the interperter run. */

            // Some Testing
            _testOutputHelper.WriteLine(
                $"Hello"
            );
        }
    }
}