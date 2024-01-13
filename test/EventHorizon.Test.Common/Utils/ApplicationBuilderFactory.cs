namespace EventHorizon.Test.Common;

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Moq;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderFactory
{
    public struct ApplicationBuilderMocks
    {
        public Mock<IServiceProvider> ServiceProviderMock { get; }
        public Mock<IServiceScope> ServiceScopeMock { get; }
        public Mock<IServiceScopeFactory> ServiceScopeFactoryMock { get; }
        public Mock<IServiceProvider> ApplicationServicesMock { get; }
        public Mock<IApplicationBuilder> ApplicationBuilderMock { get; }

        public ApplicationBuilderMocks(
            Mock<IServiceProvider> serviceProviderMock,
            Mock<IServiceScope> serviceScopeMock,
            Mock<IServiceScopeFactory> serviceScopeFactoryMock,
            Mock<IServiceProvider> applicationServicesMock,
            Mock<IApplicationBuilder> applicationBuilderMock
        )
        {
            ServiceProviderMock = serviceProviderMock;
            ServiceScopeMock = serviceScopeMock;
            ServiceScopeFactoryMock = serviceScopeFactoryMock;
            ApplicationServicesMock = applicationServicesMock;
            ApplicationBuilderMock = applicationBuilderMock;
        }
    }

    public static ApplicationBuilderMocks CreateApplicationBuilder()
    {
        var serviceProviderMock = new Mock<IServiceProvider>();

        var serviceScopeMock = new Mock<IServiceScope>();
        serviceScopeMock.SetupGet(
            serviceScope => serviceScope.ServiceProvider
        ).Returns(
            serviceProviderMock.Object
        );

        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock.Setup(
            mock => mock.CreateScope()
        ).Returns(
            serviceScopeMock.Object
        );

        var applicationServicesMock = new Mock<IServiceProvider>();
        applicationServicesMock.Setup(
            mock => mock.GetService(
                typeof(IServiceScopeFactory)
            )
        ).Returns(
            serviceScopeFactoryMock.Object
        );

        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        applicationBuilderMock.Setup(
            mock => mock.ApplicationServices
        ).Returns(
            applicationServicesMock.Object
        );

        return new ApplicationBuilderMocks(
            serviceProviderMock,
            serviceScopeMock,
            serviceScopeFactoryMock,
            applicationServicesMock,
            applicationBuilderMock
        );
    }
}
