using System.Collections.Generic;
using EventHorizon.Game.I18n.Lookup;
using EventHorizon.Game.I18n.Model;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.I81n.Repository
{
    public class I18nLookupRepositoryTests
    {
        [Fact]
        public void TestLookup_ShouldReturnExpectedRepository()
        {
            //Given
            var locale = "en_US";
            var key = "HelloWorld";
            var keyValue = "hi";

            var expected = new Dictionary<string, string>()
                {{
                    key, keyValue
                }};

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                expected
            );

            var actual = i18nLookupRepository.GetRepository(
                locale
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnEmptyRepositoryWhenNotIsFound()
        {
            //Given
            var locale = "en_US";

            //When
            var i18nLookupRepository = new I18nLookupRepository();

            var actual = i18nLookupRepository.GetRepository(
                locale
            );

            //Then
            Assert.Empty(
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnNotFoundTranslationWhenNotFound()
        {
            //Given
            var expected = "[[HelloWorld (NOT_FOUND)]]";

            var locale = "en_US";
            var key = "HelloWorld";

            //When
            var i18nLookupRepository = new I18nLookupRepository();

            var actual = i18nLookupRepository.Lookup(
                locale,
                key
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnExpectedTranslationWhenFound()
        {
            //Given
            var expected = "Hello, World!";

            var locale = "en_US";
            var key = "HelloWorld";

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                new Dictionary<string, string>()
                {{
                    key, expected
                }}
            );

            var actual = i18nLookupRepository.Lookup(
                locale,
                key
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnExpectedResolvedKeyValueTranslationWhenFound()
        {
            //Given
            var expected = "Hello, World!";

            var locale = "en_US";
            var key = "HelloWorld";

            var firstToken = "FIRST";
            var firstValue = "Hello";
            var secondToken = "second";
            var secondValue = "World";

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                new Dictionary<string, string>()
                {{
                    key, "{FIRST}, {second}!"
                }}
            );

            var actual = i18nLookupRepository.Resolve(
                locale,
                key,
                new I18nTokenValue
                {
                    Token = firstToken,
                    Value = firstValue
                },
                new I18nTokenValue
                {
                    Token = secondToken,
                    Value = secondValue
                }
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}