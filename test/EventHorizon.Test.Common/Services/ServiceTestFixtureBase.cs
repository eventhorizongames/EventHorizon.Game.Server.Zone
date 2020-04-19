namespace EventHorizon.Test.Common.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit.Abstractions;

    public class ServiceTestFixtureBase
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        public readonly ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;

        protected ServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider == null
                    ? _serviceProvider = _serviceCollection.BuildServiceProvider()
                    : _serviceProvider;
            }
        }

        public ServiceTestFixtureBase(
            ITestOutputHelper testOutputHelper
        )
        {
            _testOutputHelper = testOutputHelper;
            _serviceCollection = new ServiceCollection();
        }
    }
}