using Xunit.Abstractions;

namespace EventHorizon.Tests.TestUtils
{
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