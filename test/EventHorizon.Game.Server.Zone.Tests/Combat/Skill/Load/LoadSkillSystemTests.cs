using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Json.Impl;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using Xunit;
using Xunit.Abstractions;

namespace EventHorizon.Game.Server.Zone.Tests.Combat.Skill.Load
{
    public class LoadSkillSystemTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public LoadSkillSystemTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestLoadingSkillsFromFile()
        {
            // Given
            var fileLoader = new JsonFileLoader();
            var skillList = await fileLoader.GetFile<SkillListFile>(@"C:\Users\codya\Source\Repo\EventHorizon.Game.Server.Zone\src\EventHorizon.Plugin.Zone.System.Combat\Assets\Combat.Skills.json");

            // When
            _testOutputHelper.WriteLine("Hello");

            // Then
        }
    }
}