namespace EventHorizon.Test.Common.Utils
{
    using System.Diagnostics.CodeAnalysis;
    using Xunit.Abstractions;

    [ExcludeFromCodeCoverage]
    public class TestFixtureBase
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        public TestFixtureBase(
            ITestOutputHelper testOutputHelper
        )
        {
            _testOutputHelper = testOutputHelper;
        }
    }
}
