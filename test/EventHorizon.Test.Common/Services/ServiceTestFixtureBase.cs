namespace EventHorizon.Test.Common.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit.Abstractions;

    [ExcludeFromCodeCoverage]
    public class ServiceTestFixtureBase
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        public readonly ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;

        protected ServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider ??= _serviceCollection.BuildServiceProvider();
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