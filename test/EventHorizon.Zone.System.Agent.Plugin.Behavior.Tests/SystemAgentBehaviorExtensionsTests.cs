namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests;

using global::System;
using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common.Utils;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;
using global::System.Threading;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Xunit.Abstractions;

public class SystemAgentBehaviorExtensionsTests : TestFixtureBase
{
    public SystemAgentBehaviorExtensionsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) { }

    [Fact]
    public void TestAddServerSetup_ShouldAddExpectedServices()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        SystemAgentPluginBehaviorExtensions.AddSystemAgentPluginBehavior(serviceCollectionMock);

        // Then
        Assert.NotEmpty(serviceCollectionMock);
        Assert.Collection(
            serviceCollectionMock,
            service =>
            {
                Assert.Equal(typeof(ITimerTask), service.ServiceType);
                Assert.Equal(
                    typeof(RunPendingActorBehaviorTicksTimer),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(typeof(ActorBehaviorTreeRepository), service.ServiceType);
                Assert.Equal(
                    typeof(InMemoryActorBehaviorTreeRepository),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(typeof(ActorBehaviorTickQueue), service.ServiceType);
                Assert.Equal(
                    typeof(InMemoryActorBehaviorTickQueue),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(typeof(BehaviorInterpreterMap), service.ServiceType);
                Assert.Equal(
                    typeof(BehaviorInterpreterInMemoryMap),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(typeof(BehaviorInterpreterKernel), service.ServiceType);
                Assert.Equal(
                    typeof(BehaviorInterpreterDoWhileKernel),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(typeof(ActionBehaviorInterpreter), service.ServiceType);
                Assert.Equal(typeof(ActionInterpreter), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ConditionBehaviorInterpreter), service.ServiceType);
                Assert.Equal(typeof(ConditionInterpreter), service.ImplementationType);
            }
        );
    }

    [Fact]
    public void TestUseSetupServer_ShouldSendAndPublishExpectedEvent()
    {
        // Given
        var expectedLoadAgentBehaviorSystemEvent = new LoadAgentBehaviorSystem();

        var mediatorMock = new Mock<IMediator>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object);
        var serviceScopeMock = new Mock<IServiceScope>();
        serviceScopeMock
            .SetupGet(serviceScope => serviceScope.ServiceProvider)
            .Returns(serviceProviderMock.Object);
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock
            .Setup(serviceScopeFactory => serviceScopeFactory.CreateScope())
            .Returns(serviceScopeMock.Object);
        var applicationServicesMock = new Mock<IServiceProvider>();
        applicationServicesMock
            .Setup(
                applicationServices =>
                    applicationServices.GetService(typeof(IServiceScopeFactory))
            )
            .Returns(serviceScopeFactoryMock.Object);

        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        applicationBuilderMock
            .Setup(a => a.ApplicationServices)
            .Returns(applicationServicesMock.Object);

        // When
        SystemAgentPluginBehaviorExtensions.UseSystemAgentPluginBehavior(
            applicationBuilderMock.Object
        );

        // Then
        mediatorMock.Verify(
            mediator =>
                mediator.Send<IRequest>(
                    expectedLoadAgentBehaviorSystemEvent,
                    CancellationToken.None
                )
        );
    }
}
