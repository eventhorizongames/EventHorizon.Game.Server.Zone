namespace EventHorizon.Extensions.Tests;

using System;

using AutoFixture.Xunit2;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using Xunit;


public class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void ThrowsInvalidOperationExceptionWhenServiceScopeFactoryServiceNotFound()
    {
        // Given
        var serviceProviderMock = new Mock<IServiceProvider>();
        var applicationBuilderMock = new Mock<IApplicationBuilder>();

        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IServiceScopeFactory))
        ).Returns(
            default(IServiceScopeFactory)
        );
        applicationBuilderMock.Setup(
            mock => mock.ApplicationServices
        ).Returns(
            serviceProviderMock.Object
        );

        // When
        Action action = () => ApplicationBuilderExtensions.CreateServiceScope(
            applicationBuilderMock.Object
        );

        // Then
        action.Should().Throw<InvalidOperationException>()
            .WithMessage(
                $"{typeof(IServiceScopeFactory)} was not found"
            );
    }

    [Fact]
    public void ThrowsInvalidOperationExceptionWhenMediatorServiceIsNotFound()
    {
        // Given
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var commandMock = new Mock<IRequest>();

        serviceScopeFactoryMock.Setup(
            mock => mock.CreateScope()
        ).Returns(
            serviceScopeMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IServiceScopeFactory))
        ).Returns(
            serviceScopeFactoryMock.Object
        );
        applicationBuilderMock.Setup(
            mock => mock.ApplicationServices
        ).Returns(
            serviceProviderMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            default(IMediator)
        );
        serviceScopeMock.Setup(
            mock => mock.ServiceProvider
        ).Returns(
            serviceProviderMock.Object
        );

        // When
        Action action = () => ApplicationBuilderExtensions.SendMediatorCommand(
            applicationBuilderMock.Object,
            commandMock.Object
        );

        // Then
        action.Should().Throw<InvalidOperationException>()
            .WithMessage(
                $"{typeof(IMediator)} was not found"
            );
    }

    [Fact]
    public void ThrowsArgumentNullExceptionWhenCommandWithNoReturnTypeIsSent()
    {
        // Given
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        var serviceScopeMock = new Mock<IServiceScope>();

        serviceScopeFactoryMock.Setup(
            mock => mock.CreateScope()
        ).Returns(
            serviceScopeMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IServiceScopeFactory))
        ).Returns(
            serviceScopeFactoryMock.Object
        );
        applicationBuilderMock.Setup(
            mock => mock.ApplicationServices
        ).Returns(
            serviceProviderMock.Object
        );
        serviceScopeMock.Setup(
            mock => mock.ServiceProvider
        ).Returns(
            serviceProviderMock.Object
        );

        // When
        Action action = () => ApplicationBuilderExtensions.SendMediatorCommand(
            applicationBuilderMock.Object,
            null
        );

        // Then
        action.Should().Throw<ArgumentNullException>()
            .WithMessage(
                $"Value cannot be null. (Parameter 'command')"
            );
    }

    [Fact]
    public void ThrowsInvalidOperationExceptionWhenMediatorServiceIsNotFoundForCommandWithResult()
    {
        // Given
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var commandMock = new Mock<IRequest>();

        serviceScopeFactoryMock.Setup(
            mock => mock.CreateScope()
        ).Returns(
            serviceScopeMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IServiceScopeFactory))
        ).Returns(
            serviceScopeFactoryMock.Object
        );
        applicationBuilderMock.Setup(
            mock => mock.ApplicationServices
        ).Returns(
            serviceProviderMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            default(IMediator)
        );
        serviceScopeMock.Setup(
            mock => mock.ServiceProvider
        ).Returns(
            serviceProviderMock.Object
        );

        // When
        Action action = () => ApplicationBuilderExtensions.SendMediatorCommand<IRequest<string>, string>(
            applicationBuilderMock.Object,
            null
        );

        // Then
        action.Should().Throw<InvalidOperationException>()
            .WithMessage(
                $"{typeof(IMediator)} was not found"
            );
    }

    [Fact]
    public void ThrowsArgumentNullExceptionWhenCommandWithReturnTypeIsSent()
    {
        // Given
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var mediatorMock = new Mock<IMediator>();

        serviceScopeFactoryMock.Setup(
            mock => mock.CreateScope()
        ).Returns(
            serviceScopeMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IServiceScopeFactory))
        ).Returns(
            serviceScopeFactoryMock.Object
        );
        applicationBuilderMock.Setup(
            mock => mock.ApplicationServices
        ).Returns(
            serviceProviderMock.Object
        );
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            mediatorMock.Object
        );
        serviceScopeMock.Setup(
            mock => mock.ServiceProvider
        ).Returns(
            serviceProviderMock.Object
        );

        // When
        Action action = () => ApplicationBuilderExtensions.SendMediatorCommand<IRequest<string>, string>(
            applicationBuilderMock.Object,
            null
        );

        // Then
        action.Should().Throw<ArgumentNullException>()
            .WithMessage(
                $"Parameter 'command' of 'MediatR.IRequest`1[System.String]' Type cannot be null. (Parameter 'command')"
            );
    }
}
