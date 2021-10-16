namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Tests.Consolidate
{
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Consolidate;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Xunit;

    public class ConsolidatServerScriptsCommandHandlerTests
    {
        private static string NL { get; } = Environment.NewLine;

        [Fact]
        public async Task ShouldReturnExpectedCompiled()
        {
            // Given
            var expectedScriptClasses = string.Join(
                NL,
                "// === FILE_START ===",
                "// Script Id: path_file-name",
                "script-content-001",
                string.Empty,
                "// === FILE_END ===",
                "// === FILE_START ===",
                "// Script Id: path_file-name",
                "script-content-002",
                string.Empty,
                "// === FILE_END ===",
                "// === FILE_START ===",
                "// Script Id: path_file-name",
                "script-content-003",
                string.Empty,
                "// === FILE_END ==="
            );
            var expectedConsolidatedScripts = string.Join(
                NL,
                "// === FILE_START ===",
                "// Script Id: path_file-name",
                "script-content-001",
                string.Empty,
                "// === FILE_END ===",
                "// === FILE_START ===",
                "// Script Id: path_file-name",
                "script-content-002",
                string.Empty,
                "// === FILE_END ===",
                "// === FILE_START ===",
                "// Script Id: path_file-name",
                "script-content-003",
                string.Empty,
                "// === FILE_END ==="
            );

            var scripts = new List<ServerScriptDetails>
            {
                new ServerScriptDetails(
                    "file-name",
                    "path",
                    "script-content-001"
                ),
                new ServerScriptDetails(
                    "file-name",
                    "path",
                    "script-content-002"
                ),
                new ServerScriptDetails(
                    "file-name",
                    "path",
                    "script-content-003"
                ),
            };

            // When
            var handler = new ConsolidateServerScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateServerScriptsCommand(
                    scripts
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result.ScriptClasses
                .Trim()
                .Should().Be(expectedScriptClasses);
            actual.Result.ConsolidatedScripts
                .Trim()
                .Should().Be(expectedConsolidatedScripts);
            actual.Result.UsingList
                .Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnScriptStringReplacedWithNameWhenContentContainsScriptNameIdentifier()
        {
            // Given
            var scriptNameIdentifier = "__SCRIPT__";
            var scriptPath = "ScriptPath";
            var scriptName = "ScriptName";
            var scriptString = $"{scriptNameIdentifier}";
            var expected = string.Join(
                NL,
                "// === FILE_START ===",
                $"// Script Id: {scriptPath}_ScriptName",
                $"{scriptPath}_ScriptName",
                string.Empty,
                "// === FILE_END ==="
            );

            var scripts = new List<ServerScriptDetails>
            {
                new ServerScriptDetails(
                    scriptName,
                    scriptPath,
                    scriptString
                ),
            };

            // When
            var handler = new ConsolidateServerScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateServerScriptsCommand(
                    scripts
                ),
                CancellationToken.None
            );

            // Then
            actual.Result.ConsolidatedScripts
                .Trim()
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldCleanupScriptNameWhenNameContainsScriptExtension()
        {
            // Given
            var scriptNameIdentifier = "__SCRIPT__";
            var scriptPath = "ScriptPath";
            var scriptName = "ScriptName.csx";
            var scriptString = $"{scriptNameIdentifier}";
            var expected = string.Join(
                NL,
                "// === FILE_START ===",
                $"// Script Id: {scriptPath}_ScriptName.csx",
                $"{scriptPath}_ScriptName",
                string.Empty,
                "// === FILE_END ==="
            );

            var scripts = new List<ServerScriptDetails>
            {
                new ServerScriptDetails(
                    scriptName,
                    scriptPath,
                    scriptString
                ),
            };

            // When
            var handler = new ConsolidateServerScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateServerScriptsCommand(
                    scripts
                ),
                CancellationToken.None
            );

            // Then
            actual.Result.ConsolidatedScripts
                .Trim()
                .Should().Be(expected);
        }

        [Fact]
        public async Task ShouldReturnListOfUnsingStatementsWhen()
        {
            // Given
            var scriptNameIdentifier = "__SCRIPT__";
            var scriptUsing = "using ThisIsAUsing;";
            var scriptPath = "ScriptPath";
            var scriptName = "ScriptName.csx";
            var scriptContent = string.Join(
                Environment.NewLine,
                scriptUsing,
                $"{scriptNameIdentifier}"
            );
            var expected = new List<string>
            {
                scriptUsing
            };
            var scripts = new List<ServerScriptDetails>
            {
                new ServerScriptDetails(
                    scriptName,
                    scriptPath,
                    scriptContent
                ),
            };

            // When
            var handler = new ConsolidateServerScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateServerScriptsCommand(
                    scripts
                ),
                CancellationToken.None
            );

            // Then
            actual.Result.UsingList
                .Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task ShouldReturnDistinctListOfUsingsWhenMultipleScriptsUseSameUsing()
        {
            // Given
            var scriptNameIdentifier = "__SCRIPT__";
            var scriptUsing = "using ThisIsAUsing;";
            var scriptPath = "ScriptPath";
            var scriptName = "ScriptName.csx";
            var scriptContent = string.Join(
                Environment.NewLine,
                scriptUsing,
                $"{scriptNameIdentifier}"
            );
            var script2Using = "using ThisIsAUsing;";
            var script2Path = "ScriptPath";
            var script2Name = "ScriptName.csx";
            var script2Content = string.Join(
                Environment.NewLine,
                script2Using,
                $"{scriptNameIdentifier}"
            );
            var expected = new List<string>
            {
                scriptUsing
            };
            var scripts = new List<ServerScriptDetails>
            {
                new ServerScriptDetails(
                    scriptName,
                    scriptPath,
                    scriptContent
                ),
                new ServerScriptDetails(
                    script2Name,
                    script2Path,
                    script2Content
                ),
            };

            // When
            var handler = new ConsolidateServerScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateServerScriptsCommand(
                    scripts
                ),
                CancellationToken.None
            );

            // Then
            actual.Result.UsingList
                .Should().BeEquivalentTo(expected);
        }
    }
}
