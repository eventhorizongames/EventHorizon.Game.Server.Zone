using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Admin.Plugin.Command.Run;
using EventHorizon.Zone.System.Admin.Plugin.Command.State;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Run
{
    public class RunAdminCommandScriptHandlerTests
    {
        [Fact]
        public async Task TestShouldNotRunAnyServerScriptsWhenCommandScriptsListIsEmpty()
        {
            // Given
            var expected = new RunServerScriptCommand(
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, object>>()
            );
            var emptyList = new List<AdminCommandInstance>();
            var connectionId = "connection-id";
            var command = "command";
            var adminCommandMock = new Mock<IAdminCommand>();

            var loggerMock = new Mock<ILogger<RunAdminCommandScriptHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<AdminCommandRepository>();

            repositoryMock.Setup(
                mock => mock.Where(
                    command
                )
            ).Returns(
                emptyList
            );

            // When
            var handler = new RunAdminCommandScriptHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    adminCommandMock.Object,
                    repositoryMock.Object
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    It.IsAny<CancellationToken>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldRunServerScriptWhenRepositoryContainsAtLeastOneCommandScript()
        {
            // Given
            var scriptFile = "script-file";
            var connectionId = "connection-id";
            var command = "command";
            var data = new Dictionary<string, object>();
            var adminCommand = new Mock<IAdminCommand>();
            Expression<Func<RunServerScriptCommand, bool>> expected = commandEvent =>
                commandEvent.Id == scriptFile
                &&
                commandEvent.Data.Count == 2
                &&
                commandEvent.Data.ContainsKey(
                    "Command"
                )
                &&
                commandEvent.Data["Command"].Equals(
                    adminCommand.Object
                )
                &&
                commandEvent.Data.ContainsKey(
                    "Data"
                )
                &&
                commandEvent.Data["Data"].Equals(
                    data
                );

            var loggerMock = new Mock<ILogger<RunAdminCommandScriptHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<AdminCommandRepository>();

            adminCommand.Setup(
                mock => mock.Command
            ).Returns(
                command
            );

            repositoryMock.Setup(
                mock => mock.Where(
                    command
                )
            ).Returns(
                new List<AdminCommandInstance>
                {
                    new AdminCommandInstance
                    {
                        Command = command,
                        ScriptFile = scriptFile
                    }
                }
            );

            // When
            var handler = new RunAdminCommandScriptHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    adminCommand.Object,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        expected
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldRespondWithScriptResponseWhenRepositoryContainsAtLeastOneCommandScript()
        {
            // Given
            var scriptFile = "script-file";
            var connectionId = "connection-id";
            var command = "command";
            var rawCommand = "raw-command";
            var responseMessage = "response-message";
            var data = new Dictionary<string, object>();
            var adminCommand = new Mock<IAdminCommand>();
            Expression<Func<RespondToAdminCommand, bool>> expected = commandEvent =>
                commandEvent.ConnectionId == connectionId
                &&
                commandEvent.Response.CommandFunction == command
                &&
                commandEvent.Response.RawCommand == rawCommand
                &&
                commandEvent.Response.Success
                &&
                commandEvent.Response.Message.Equals(
                    responseMessage
                );
            var adminCommandResponse = new AdminCommandScriptResponse(
                true,
                responseMessage
            );

            var loggerMock = new Mock<ILogger<RunAdminCommandScriptHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<AdminCommandRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                adminCommandResponse
            );

            adminCommand.Setup(
                mock => mock.Command
            ).Returns(
                command
            );

            adminCommand.Setup(
                mock => mock.RawCommand
            ).Returns(
                rawCommand
            );

            repositoryMock.Setup(
                mock => mock.Where(
                    command
                )
            ).Returns(
                new List<AdminCommandInstance>
                {
                    new AdminCommandInstance
                    {
                        Command = command,
                        ScriptFile = scriptFile
                    }
                }
            );

            // When
            var handler = new RunAdminCommandScriptHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    adminCommand.Object,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RespondToAdminCommand>(
                        expected
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldNotStopProcessingCommandScriptsWhenOnThrowsException()
        {
            // Given
            var exceptionThrowingScriptFile = "this-script-will-throw-an-exception";
            var scriptFile = "script-file";
            var connectionId = "connection-id";
            var command = "command";
            var rawCommand = "raw-command";
            var responseMessage = "response-message";
            var data = new Dictionary<string, object>();
            var adminCommand = new Mock<IAdminCommand>();
            Expression<Func<RespondToAdminCommand, bool>> expected = commandEvent =>
                commandEvent.ConnectionId == connectionId
                &&
                commandEvent.Response.CommandFunction == command
                &&
                commandEvent.Response.RawCommand == rawCommand
                &&
                commandEvent.Response.Success
                &&
                commandEvent.Response.Message.Equals(
                    responseMessage
                );
            var adminCommandResponse = new AdminCommandScriptResponse(
                true,
                responseMessage
            );

            var loggerMock = new Mock<ILogger<RunAdminCommandScriptHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<AdminCommandRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        scriptCommand => scriptCommand.Id == exceptionThrowingScriptFile
                    ),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "Invalid Script"
                )
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        scriptCommand => scriptCommand.Id == scriptFile
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                adminCommandResponse
            );

            adminCommand.Setup(
                mock => mock.Command
            ).Returns(
                command
            );

            adminCommand.Setup(
                mock => mock.RawCommand
            ).Returns(
                rawCommand
            );

            repositoryMock.Setup(
                mock => mock.Where(
                    command
                )
            ).Returns(
                new List<AdminCommandInstance>
                {
                    new AdminCommandInstance
                    {
                        Command = command,
                        ScriptFile = exceptionThrowingScriptFile
                    },
                    new AdminCommandInstance
                    {
                        Command = command,
                        ScriptFile = scriptFile
                    }
                }
            );

            // When
            var handler = new RunAdminCommandScriptHandler(
                loggerMock.Object,
                mediatorMock.Object,
                repositoryMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    adminCommand.Object,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                ),
                Times.Exactly(2)
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RespondToAdminCommand>(),
                    CancellationToken.None
                ),
                Times.Exactly(1)
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RespondToAdminCommand>(
                        expected
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
