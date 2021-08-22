namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model.Tests.Builder
{
    using global::System;
    using global::System.Collections;
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;

    using Xunit;

    public class BuildAdminCommandTests
    {
        [Fact]
        public void TestShouldStartOfRawCommandShoudlContainCommandWhenSpaceIsFound()
        {
            // Given
            var expected = "expected-command";
            var rawCommand = "expected-command arg1 arg2 arg3";

            // When
            var actual = BuildAdminCommand.FromString(
                rawCommand
            );

            // Then
            Assert.Equal(
                expected,
                actual.Command
            );
        }

        [Fact]
        public void TestShouldEchoTheRawCommandWhenTheCommandIsCreated()
        {
            // Given
            var rawCommand = "expected-command arg1 arg2 arg3";
            var expected = rawCommand;

            // When
            var actual = BuildAdminCommand.FromString(
                rawCommand
            );

            // Then
            Assert.Equal(
                expected,
                actual.RawCommand
            );
        }

        [Theory]
        [ClassData(typeof(AdminCommandTestData))]
        public void TestRawDataWithMathingExpected(
            // Given
            string rawCommand,
            Action<string>[] expected
        )
        {
            // When
            var actual = BuildAdminCommand.FromString(
                rawCommand
            );

            // Then
            Assert.Collection(
                actual.Parts,
                expected
            );
        }

        public class AdminCommandTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    "expected-command \"arg1 in quotes\"",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1 in quotes", arg)
                    }
                };
                yield return new object[] {
                    "expected-command \"arg1 in quotes\" \"arg2 in quotes\"",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1 in quotes", arg),
                        arg => Assert.Equal("arg2 in quotes", arg)
                    }
                };
                yield return new object[] {
                    "expected-command arg1 arg2 arg3 arg4-not-containing-space",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1", arg),
                        arg => Assert.Equal("arg2", arg),
                        arg => Assert.Equal("arg3", arg),
                        arg => Assert.Equal("arg4-not-containing-space", arg)
                    }
                };
                yield return new object[] {
                    "expected-command \"arg1 in quotes\" arg2 \"arg3 in quotes\" arg4-with-no-space",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1 in quotes", arg),
                        arg => Assert.Equal("arg2", arg),
                        arg => Assert.Equal("arg3 in quotes", arg),
                        arg => Assert.Equal("arg4-with-no-space", arg)
                    }
                };
                yield return new object[] {
                    "expected-command \"arg1 in quotes\" arg2 \"arg3 in quotes\" arg4-with-no-space",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1 in quotes", arg),
                        arg => Assert.Equal("arg2", arg),
                        arg => Assert.Equal("arg3 in quotes", arg),
                        arg => Assert.Equal("arg4-with-no-space", arg)
                    }
                };
                yield return new object[] {
                    "expected-command arg1 \"arg2 in quotes\" arg3-with-no-space",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1", arg),
                        arg => Assert.Equal("arg2 in quotes", arg),
                        arg => Assert.Equal("arg3-with-no-space", arg)
                    }
                };
                yield return new object[] {
                    "expected-command arg1 \"\"arg2 in quotes\" arg3-with-no-space",
                    new Action<string>[]
                    {
                        arg => Assert.Equal("arg1", arg),
                        arg => Assert.Equal("arg2 in quotes", arg),
                        arg => Assert.Equal("arg3-with-no-space", arg)
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
