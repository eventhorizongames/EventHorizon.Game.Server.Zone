using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Performance;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Update;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Update.RunUpdateOnAllBehaviorTrees;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Update
{
    public class RunUpdateOnAllBehaviorTreesTests
    {
        [Fact]
        public async Task TestTestName()
        {
            // Given
            var treeId1 = "tree-1";
            var treeId2 = "tree-2";
            var treeId3 = "tree-3";
            var expectedRunBehaviorTreeUpdate1 = new RunBehaviorTreeUpdate(
                treeId1
            );
            var expectedRunBehaviorTreeUpdate2 = new RunBehaviorTreeUpdate(
                treeId2
            );
            var expectedRunBehaviorTreeUpdate3 = new RunBehaviorTreeUpdate(
                treeId3
            );
            var treeIdList = new List<string>()
            {
                treeId1,
                treeId2,
                treeId3
            };

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock.Setup(
                serviceProvider => serviceProvider
                    .GetService(
                        typeof(IMediator)
                    )
                ).Returns(
                    mediatorMock.Object
                );
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );

            var loggerMock = new Mock<ILogger<RunUpdateOnAllBehaviorTreesHandler>>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            actorBehaviorTreeRepositoryMock.Setup(
                moveRepository => moveRepository.TreeIdList(
                )).Returns(
                    treeIdList
                );
            serviceScopeFactoryMock.Setup(
                serviceScopeFactory => serviceScopeFactory.CreateScope(

                )).Returns(
                    serviceScopeMock.Object
                );

            // When
            var moveRegisteredAgentsHandler = new RunUpdateOnAllBehaviorTreesHandler(
                loggerMock.Object,
                actorBehaviorTreeRepositoryMock.Object,
                serviceScopeFactoryMock.Object
            );
            await moveRegisteredAgentsHandler.Handle(
                new RunUpdateOnAllBehaviorTrees(),
                CancellationToken.None
            );

            // Then
            loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains("failed Run.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedRunBehaviorTreeUpdate1,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedRunBehaviorTreeUpdate2,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedRunBehaviorTreeUpdate3,
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task OnErrorShouldLogTheTreeName()
        {
            // Given
            var treeId1 = "tree-1";
            var expectedError = new Exception(
                "Error"
            );
            var treeIdList = new List<string>()
            {
                treeId1
            };

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            mediatorMock.Setup(
                mediator => mediator.Publish(
                    It.IsAny<RunBehaviorTreeUpdate>(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                expectedError
            );

            serviceProviderMock.Setup(
                serviceProvider => serviceProvider
                    .GetService(
                        typeof(IMediator)
                    )
                ).Returns(
                    mediatorMock.Object
                );
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );

            var loggerMock = new Mock<ILogger<RunUpdateOnAllBehaviorTreesHandler>>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            actorBehaviorTreeRepositoryMock.Setup(
                moveRepository => moveRepository.TreeIdList(
                )).Returns(
                    treeIdList
                );
            serviceScopeFactoryMock.Setup(
                serviceScopeFactory => serviceScopeFactory.CreateScope(
                )).Returns(
                    serviceScopeMock.Object
                );

            // When
            var moveRegisteredAgentsHandler = new RunUpdateOnAllBehaviorTreesHandler(
                loggerMock.Object,
                actorBehaviorTreeRepositoryMock.Object,
                serviceScopeFactoryMock.Object
            );
            await moveRegisteredAgentsHandler.Handle(
                new RunUpdateOnAllBehaviorTrees(),
                CancellationToken.None
            );

            // Then
            loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Contains("tree-1 failed Run.")),
                    expectedError,
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
        }
    }
}