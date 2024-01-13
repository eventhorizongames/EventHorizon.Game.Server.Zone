namespace EventHorizon.Game.I18n.Tests;

using System;
using System.Threading;

using EventHorizon.Game.I18n.Loader;
using EventHorizon.Game.I18n.Lookup;
using EventHorizon.Test.Common.Utils;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using Xunit;

public class I18nExtensionsTests
{
    [Fact]
    public void TestAddI18n_ShouldReturnExpectedTypedImplementations()
    {
        //Given
        var serviceCollectionMock = new ServiceCollectionMock();

        //When
        I18nExtensions.AddI18n(serviceCollectionMock);
        var actual = serviceCollectionMock.Services;

        //Then
        Assert.Collection(actual,
            a => Assert.IsType<I18nLookupRepository>(a.ImplementationInstance),
            a => Assert.IsType<I18nLookupRepository>(a.ImplementationInstance),
            a => Assert.IsType<I18nLookupRepository>(a.ImplementationInstance)
        );
    }

    [Fact]
    public void TestUseI18n_ShouldSendAndPublishExpectedEvent()
    {
        // Given
        var expectedI18nLoadEvent = new I18nLoadEvent();

        var mediatorMock = new Mock<IMediator>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
        var serviceScopeMock = new Mock<IServiceScope>();
        serviceScopeMock.SetupGet(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock.Setup(a => a.CreateScope()).Returns(serviceScopeMock.Object);
        var applicationServicesMock = new Mock<IServiceProvider>();
        applicationServicesMock.Setup(a => a.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);

        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        applicationBuilderMock.Setup(a => a.ApplicationServices).Returns(applicationServicesMock.Object);

        // When
        I18nExtensions.UseI18n(applicationBuilderMock.Object);

        // Then
        mediatorMock.Verify(mediator => mediator.Publish(expectedI18nLoadEvent, CancellationToken.None));
    }
}
