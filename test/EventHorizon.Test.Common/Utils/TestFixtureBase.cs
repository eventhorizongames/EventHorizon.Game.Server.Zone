namespace EventHorizon.Test.Common.Utils
{
    using Xunit.Abstractions;

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