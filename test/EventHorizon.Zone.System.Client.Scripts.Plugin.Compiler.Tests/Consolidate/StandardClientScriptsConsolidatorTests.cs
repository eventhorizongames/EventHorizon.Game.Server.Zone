namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.Consolidate
{
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Consolidate;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using Xunit;

    public class StandardClientScriptsConsolidatorTests
    {
        [Fact]
        public void ShouldReturnSingleStringOfScriptsWhenClientScriptEnumerableIsPassedWithContent()
        {
            // Given
            var expected = string.Join(
                Environment.NewLine,
                "script-content-001",
                string.Empty,
                "script-content-002",
                string.Empty,
                "script-content-003",
                Environment.NewLine
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
            var consolidator = new StandardClientScriptsConsolidator();
            var usingList = new List<string>();
            var actual = consolidator.IntoSingleTemplatedString(
                scripts,
                ref usingList
            );

            // Then
            actual.Should()
                .Be(
                    expected
                );
        }

        [Fact]
        public void ShouldReturnScriptStringReplacedWithNameWhenContentContainsScriptNameIdentifier()
        {
            // Given
            var scriptNameIdentifier = "__SCRIPT__";
            var scriptPath = "ScriptPath";
            var scriptName = "ScriptName";
            var scriptString = $"{scriptNameIdentifier}";
            var expected = string.Join(
                Environment.NewLine,
                $"{scriptPath}_{scriptName}",
                Environment.NewLine
            );
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
            var consolidater = new StandardClientScriptsConsolidator();
            var usingList = new List<string>();
            var actual = consolidater.IntoSingleTemplatedString(
                scripts,
                ref usingList
            );

            // Then
            actual.Should()
                .Be(
                    expected
                );
        }

        [Fact]
        public void ShouldCleanupScriptNameWhenNameContainsScriptExtension()
        {
            // Given
            var scriptNameIdentifier = "__SCRIPT__";
            var scriptPath = "ScriptPath";
            var scriptName = "ScriptName.csx";
            var scriptString = $"{scriptNameIdentifier}";
            var expected = string.Join(
                Environment.NewLine,
                $"{scriptPath}_ScriptName",
                Environment.NewLine
            );
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
            var consolidator = new StandardClientScriptsConsolidator();
            var usingList = new List<string>();
            var actual = consolidator.IntoSingleTemplatedString(
                scripts,
                ref usingList
            );

            // Then
            actual.Should()
                .Be(
                    expected
                );
        }

        [Fact]
        public void ShouldReturnListOfUnsingStatementsWhen()
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
            var consolidator = new StandardClientScriptsConsolidator();
            var actual = new List<string>();
            consolidator.IntoSingleTemplatedString(
                scripts,
                ref actual
            );

            // Then
            actual.Should()
                .BeEquivalentTo(
                    expected
                );
        }
    }
}