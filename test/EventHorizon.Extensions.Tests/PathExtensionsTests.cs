namespace EventHorizon.Extensions.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using Xunit;

    public class PathExtensionsTests
    {
        [Theory]
        [ClassData(typeof(GeneratorValidTestData))]
        public void TestTheoryOfValidData(
            // Given
            TestData testData
        )
        {
            // When
            var actual = PathExtensions.MakePathRelative(
                testData.FromPath,
                testData.ToPath
            );

            // Then
            Assert.Equal(
                testData.Expected,
                actual
            );
        }

        public class GeneratorValidTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new()
            {
                new object[] {
                    new TestData(
                        Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "RootDirectory"
                        ),
                        Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "RootDirectory",
                            "OffRootDirectory"
                        ),
                        "OffRootDirectory"
                    ),
                },
                new object[] {
                    new TestData(
                        Path.Combine(
                            "RootDirectory"
                        ),
                        Path.Combine(
                            "RootDirectory",
                            "OffRootDirectory"
                        ),
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        "http://localhost/RootDirectory",
                        "http://localhost/RootDirectory/OffRootDirectory",
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        "file://localhost/RootDirectory",
                        "file://localhost/RootDirectory/OffRootDirectory",
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        "azure://localhost/RootDirectory",
                        "azure://localhost/RootDirectory/OffRootDirectory",
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        "azure://",
                        "azure://localhost/RootDirectory/OffRootDirectory",
                        Path.Combine(
                            "localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        )
                    )
                },
                new object[] {
                    new TestData(
                        "file:///localhost/RootDirectory",
                        "file:///localhost/RootDirectory/OffRootDirectory",
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        "localhost/RootDirectory",
                        "file://localhost/RootDirectory/OffRootDirectory",
                        Path.Combine(
                            "..",
                            "..",
                            "file:",
                            "localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        )
                    )
                },
                new object[] {
                    new TestData(
                        "/RootDirectory",
                        "file://localhost/RootDirectory/OffRootDirectory",
                        Path.Combine(
                            "..",
                            "file:",
                            "localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        )
                    )
                },
                new object[] {
                    new TestData(
                        Path.Combine(
                            "http://localhost",
                            "RootDirectory"
                        ),
                        Path.Combine(
                            "http://localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        ),
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        Path.Combine(
                            Path.DirectorySeparatorChar.ToString(),
                            "localhost",
                            "RootDirectory"
                        ),
                        Path.Combine(
                            Path.DirectorySeparatorChar.ToString(),
                            "localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        ),
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        Path.Combine(
                            "localhost",
                            "RootDirectory"
                        ),
                        Path.Combine(
                            Path.DirectorySeparatorChar.ToString(),
                            "localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        ),
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        Path.Combine(
                            Path.DirectorySeparatorChar.ToString(),
                            "localhost",
                            "RootDirectory",
                            "OffRootDirectory"
                        ),
                        Path.Combine(
                            "localhost",
                            "RootDirectory"
                        ),
                        ".."
                    )
                },
                new object[] {
                    new TestData(
                        "file://localhost/RootDirectory",
                        "file://localhost/RootDirectory/OffRootDirectory",
                        "OffRootDirectory"
                    )
                },
                new object[] {
                    new TestData(
                        "file://localhost/RootDirectory/OffRootDirectory",
                        "file://localhost/RootDirectory",
                        ".."
                    )
                },
                new object[] {
                    new TestData(
                        "file://localhost/RootDirectory/OffRootDirectory",
                        "file://localhost/",
                        Path.Combine(
                            "..",
                            ".."
                        )
                    )
                },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public class TestData
        {
            public TestData(
                string fromPath,
                string toPath,
                string expected
            )
            {
                FromPath = fromPath;
                ToPath = toPath;
                Expected = expected;
            }

            public string FromPath { get; }
            public string ToPath { get; }
            public string Expected { get; }
        }
    }
}
