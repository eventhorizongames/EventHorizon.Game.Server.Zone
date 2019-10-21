using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.RandomNumber;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.System;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.System
{
    public class SystemServerScriptTests
    {
        [Fact]
        public async Task TestShouldReturnResponseWithPassedInDataWhenRunningCreatedScript()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptAsString = LoadScriptFileAsString(
                AppDomain.CurrentDomain.BaseDirectory,
                "System",
                "TestingScripts",
                "TestSystemServerScript.csx"
            );
            var scriptMessage = "Script Message";
            var expected = scriptMessage;
            var data = new Dictionary<string, object>
            {
                { "ScriptMessage", scriptMessage }
            };

            var serverScriptServices = new SystemServerScriptServices(
                new Mock<IMediator>().Object,
                new Mock<IRandomNumberGenerator>().Object,
                new Mock<IDateTimeService>().Object,
                new Mock<I18nLookup>().Object
            );

            // When
            var serverScript = SystemServerScript.Create(
                fileName,
                path,
                scriptAsString,
                new List<Assembly>(),
                new List<string>()
            );
            var actual = await serverScript.Run(
                serverScriptServices,
                data
            );

            // Then
            Assert.True(
                actual.Success
            );
            Assert.Equal(
                expected,
                actual.Message
            );
        }

        [Fact]
        public async Task TestShouldNotFailToRetrieveDataWhenDataKeyDoesNotExist()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptAsString = LoadScriptFileAsString(
                AppDomain.CurrentDomain.BaseDirectory,
                "System",
                "TestingScripts",
                "TestSystemServerScript.csx"
            );
            var expected = new SystemServerScriptResponse(
                true,
                null
            );
            var data = new Dictionary<string, object>();

            var serverScriptServices = new SystemServerScriptServices(
                new Mock<IMediator>().Object,
                new Mock<IRandomNumberGenerator>().Object,
                new Mock<IDateTimeService>().Object,
                new Mock<I18nLookup>().Object
            );

            // When
            var serverScript = SystemServerScript.Create(
                fileName,
                path,
                scriptAsString,
                new List<Assembly>(),
                new List<string>()
            );
            var actual = await serverScript.Run(
                serverScriptServices,
                data
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task TestShouldThrowServerScriptNotFoundWhenScriptsIdIsInvalid()
        {
            // Given
            var data = new Dictionary<string, object>();

            var serverScriptServices = new SystemServerScriptServices(
                new Mock<IMediator>().Object,
                new Mock<IRandomNumberGenerator>().Object,
                new Mock<IDateTimeService>().Object,
                new Mock<I18nLookup>().Object
            );

            // When
            var serverScript = new SystemServerScript();
            Func<Task> action = async () => await serverScript.Run(
                serverScriptServices,
                data
            );

            // Then
            await Assert.ThrowsAsync<ServerScriptNotFound>(
                action
            );
        }

        [Fact]
        public async Task TestShouldHaveAccessToServicesWhenAccessingFromInsideRunningScript()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptAsString = LoadScriptFileAsString(
                AppDomain.CurrentDomain.BaseDirectory,
                "System",
                "TestingScripts",
                "TestSystemServerScriptAllServices.csx"
            );
            var now = DateTime.Now;
            var locale = "en_US";
            var i18nKey = "i18nKey";
            var randomMaxValue = 1234;

            var expectedNow = now.ToString();
            var expectedRandom = 1234;
            var expectedI18n = "I18nValue";
            var scriptMessage = $"Random: {expectedRandom} | DateTime: {expectedNow} | I18n: {expectedI18n}";
            var expected = new SystemServerScriptResponse(
                true,
                scriptMessage
            );
            var expectedMediatorCommand = new LoadServerScriptsCommand();
            var data = new Dictionary<string, object>
            {
                { "RandomMaxValue", randomMaxValue },
                { "Locale", locale },
                { "I18nKey", i18nKey },
            };

            var mediatorMock = new Mock<IMediator>();
            var randomMock = new Mock<IRandomNumberGenerator>();
            randomMock.Setup(
                mock => mock.Next(
                    randomMaxValue
                )
            ).Returns(
                expectedRandom
            );
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            dateTimeServiceMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );
            var i18nLookupMock = new Mock<I18nLookup>();
            i18nLookupMock.Setup(
                mock => mock.Lookup(
                    locale,
                    i18nKey
                )
            ).Returns(
                expectedI18n
            );

            // When
            var serverScriptServices = new SystemServerScriptServices(
                mediatorMock.Object,
                randomMock.Object,
                dateTimeServiceMock.Object,
                i18nLookupMock.Object
            );
            var serverScript = SystemServerScript.Create(
                fileName,
                path,
                scriptAsString,
                new List<Assembly>(),
                new List<string>()
            );
            var actual = await serverScript.Run(
                serverScriptServices,
                data
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedMediatorCommand,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public void TestShouldThrowInvalidOperationExecptionWhenWhenContainsScriptsFailsToCompile()
        {
            // Given
            var fileName = "file-name";
            var path = "path";
            var scriptAsString = LoadScriptFileAsString(
                AppDomain.CurrentDomain.BaseDirectory,
                "System",
                "TestingScripts",
                "TestSystemServerScriptThrowsException.txt"
            );
            var expected = "Exception with path | file-name";

            // When
            Func<ServerScript> action = () => SystemServerScript.Create(
                fileName,
                path,
                scriptAsString,
                new List<Assembly>(),
                new List<string>()
            );
            var actual = Assert.Throws<InvalidOperationException>(
                action
            );

            // Then
            Assert.Equal(
                expected,
                actual.Message
            );
        }

        private string LoadScriptFileAsString(
            params string[] filePath
        )
        {
            return File.ReadAllText(
                Path.Combine(
                    filePath
                )
            );
        }
    }
}