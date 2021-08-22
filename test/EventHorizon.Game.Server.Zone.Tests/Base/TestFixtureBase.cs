namespace EventHorizon.Game.Server.Zone.Tests.Base
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
