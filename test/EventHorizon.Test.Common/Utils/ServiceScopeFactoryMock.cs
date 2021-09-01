namespace EventHorizon.Test.Common.Utils
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    public class ServiceScopeFactoryMock
        : Mock<IServiceScopeFactory>
    {
        public Mock<IServiceScope> ServiceScopeMock { get; }
        public Mock<IServiceProvider> ServiceProviderMock { get; }

        public ServiceScopeFactoryMock(
            Mock<IServiceScope> serviceScopeMock,
            Mock<IServiceProvider> serviceProviderMock
        )
        {
            ServiceScopeMock = serviceScopeMock;
            ServiceProviderMock = serviceProviderMock;

            Setup(
                scopeFactory => scopeFactory.CreateScope()
            ).Returns(
                ServiceScopeMock.Object
            );
            ServiceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
            ).Returns(
                ServiceProviderMock.Object
            );
        }

        public ServiceScopeFactoryMock WithMock<T>(
            Mock<T> mock
        ) where T : class
        {
            ServiceProviderMock.Setup(
                serviceProvider => serviceProvider.GetService(
                    typeof(T)
                )
            ).Returns(
                mock.Object
            );

            return this;
        }
    }
}
