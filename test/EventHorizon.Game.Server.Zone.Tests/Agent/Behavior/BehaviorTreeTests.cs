using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Tests.Base;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Interpreter;
using EventHorizon.Zone.System.Agent.Behavior.Interpreters;
using Moq;
using MediatR;
using System.Threading;
using EventHorizon.Zone.System.Agent.Behavior.Script.Run;
using EventHorizon.Zone.System.Agent.Behavior.Script;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EventHorizon.Performance;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior
{
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
            /* Tree Data
            Load in the Testing Behavior Tree for this test. */
            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    System.IO.Path.Combine(
                        "Agent",
                        "Behavior",
                        "Data",
                        "AllNodesShouldHaveSuccessStatus.json"
                    )
                )
            );
            /* Tree Shape
            The Tree Shape is an represtation of the BT,
                this is static and helps with the navigation of the tree
                by the Interpreter. */
            var treeShape = new ActorBehaviorTreeShape(
                agentBehaviorTree
            );

            /* Actor BT Data
            This is the Data for a specific instance.
            Saved between Interpreter Ticks for specific Actor. */
            var actor = new DefaultEntity();

            // Setup Infrastructure mock
            var loggerMock = new Mock<ILogger<ActionInterpreter>>();
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
                        loggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                )
            );

            // When
            var actual = await interpreter.Tick(
                treeShape,
                actor
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
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
            var expectedScriptId1 = "Behavior_Action_FindNewMoveLocation.csx";
            var expectedScriptId2 = "Behavior_Action_MoveToLocation.csx";

            /* Tree Data
            Load in the Testing Behavior Tree for this test. */
            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    System.IO.Path.Combine(
                        "Agent",
                        "Behavior",
                        "Data",
                        "ShouldRunScriptCommandsForActionNode.json"
                    )
                )
            );
            /* Tree Shape
            The Tree Shape is an represtation of the BT,
                this is static and helps with the navigation of the tree
                by the Interpreter. */
            var treeShape = new ActorBehaviorTreeShape(
                agentBehaviorTree
            );

            /* Actor BT Data
            This is the Data for a specific instance.
            Saved between Interpreter Ticks for specific Actor. */
            var actor = new DefaultEntity();

            // Setup Infrastructure mock
            var loggerMock = new Mock<ILogger<ActionInterpreter>>();
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
                        loggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                )
            );

            // When
            var actual = await interpreter.Tick(
                treeShape,
                actor
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(node.Status, expectedStatus),
                node => Assert.Equal(node.Status, expectedStatus),
                node => Assert.Equal(node.Status, expectedStatus)
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new RunBehaviorScript(
                        actor,
                        expectedScriptId1
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new RunBehaviorScript(
                        actor,
                        expectedScriptId2
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestPlayground()
        {
            // Create a Tree
            var agentBehaviorTree = JsonConvert.DeserializeObject<SerializedAgentBehaviorTree>(
                await File.ReadAllTextAsync(
                    System.IO.Path.Combine(
                        "Agent",
                        "Behavior",
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
                agentBehaviorTree
            );

            /*  Actor BT Data
            This is the Data for a specific instance.
            Saved between Interpreter Ticks for specific Actor. */
            var actor = new DefaultEntity();



            /* Interpreter 
            Reads in the BT Shape and Actor BT Data. */



            // Setup Infrastructure mock
            var loggerMock = new Mock<ILogger<ActionInterpreter>>();
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
                        loggerMock.Object,
                        serviceScopeFactoryMock.Object
                    )
                )
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
            }

            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine("Time: {0}ms", elapsed);


            await File.WriteAllBytesAsync(
                System.IO.Path.Combine(
                    "Agent",
                    "Behavior",
                    "Data",
                    "StateSaved.json"
                ),
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        state
                    )
                )
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