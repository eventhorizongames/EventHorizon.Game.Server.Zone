namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Tests.Consolidate
{
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Xunit;

    public class ConsolidatClientScriptsCommandHandlerTests
    {
        private static string NL { get; } = Environment.NewLine;

        [Fact]
        public async Task ShouldReturnExpectedCompiled()
        {
            // Given
            var expectedScriptClasses = string.Join(
                string.Empty,
                "script-content-001",
                NL,
                NL,
                "script-content-002",
                NL,
                NL,
                "script-content-003"
            );
            var expectedConsolidatedScripts = string.Join(
                string.Empty,
                "script-content-001",
                NL,
                NL,
                "script-content-002",
                NL,
                NL,
                "script-content-003"
            );

            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-content-001"
                ),
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-content-002"
                ),
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-content-003"
                ),
            };

            // When
            var handler = new ConsolidatClientScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateClientScriptsCommand(
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
            var expected = $"{scriptPath}_{scriptName}";

            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    scriptPath,
                    scriptName,
                    scriptString
                ),
            };

            // When
            var handler = new ConsolidatClientScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateClientScriptsCommand(
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
            var expected = $"{scriptPath}_ScriptName";

            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    scriptPath,
                    scriptName,
                    scriptString
                ),
            };

            // When
            var handler = new ConsolidatClientScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateClientScriptsCommand(
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
            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    scriptPath,
                    scriptName,
                    scriptContent
                ),
            };

            // When
            var handler = new ConsolidatClientScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateClientScriptsCommand(
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
            var scripts = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    scriptPath,
                    scriptName,
                    scriptContent
                ),
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    script2Path,
                    script2Name,
                    script2Content
                ),
            };

            // When
            var handler = new ConsolidatClientScriptsCommandHandler();
            var actual = await handler.Handle(
                new ConsolidateClientScriptsCommand(
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
