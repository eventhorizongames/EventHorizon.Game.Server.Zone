namespace EventHorizon.Zone.Core.Tests.Info
{
    using System.Reflection;

    using EventHorizon.Zone.Core.Info;

    using Xunit;

    public class StandardSystemProvidedAssemblyListTests
    {
        [Fact]
        public void TestShouldReturnReadonlyListWhenAccessingListUsedDuringConstruction()
        {
            // Given
            var expected = typeof(StandardSystemProvidedAssemblyListTests).Assembly;

            // When
            var systemProvidedAssemblyList = new StandardSystemProvidedAssemblyList(
                new Assembly[]
                {
                    expected
                }
            );
            var actual = systemProvidedAssemblyList.List;

            // Then
            Assert.True(
                actual.IsReadOnly
            );
            Assert.Collection(
                actual,
                assembly => Assert.Equal(expected, assembly)
            );
        }
    }
}
