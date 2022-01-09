namespace EventHorizon.Zone.System.EntityModule.Tests.Load;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Load;
using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class LoadEntityModuleSystemCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task LoadBaseModuleScriptsIntoRepositoryWhenCommandIsHandled(
        // Given
        EntityScriptModule scriptModule,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<EntityModuleRepository> entityModuleRepositoryMock,
        [Frozen] Mock<IJsonFileLoader> jsonFileLoaderMock,
        LoadEntityModuleSystemCommandHandler handler
    )
    {
        var files = new List<StandardFileInfo>
        {
            new(
                "name",
                "directoryInfo",
                "fullName",
                "ext"
            ),
        };
        senderMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    Path.Combine(
                        serverInfoMock.Object.ClientPath,
                        "Modules",
                        "Base"
                    )
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            files
        );

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<EntityScriptModule>(
                "fullName"
            )
        ).ReturnsAsync(
            scriptModule
        );

        // When
        var actual = await handler.Handle(
            new LoadEntityModuleSystemCommand(),
            CancellationToken.None
        );

        // Then
        entityModuleRepositoryMock.Verify(
            mock => mock.AddBaseModule(
                scriptModule
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task LoadPlayerModuleScriptsIntoRepositoryWhenCommandIsHandled(
        // Given
        EntityScriptModule scriptModule,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<EntityModuleRepository> entityModuleRepositoryMock,
        [Frozen] Mock<IJsonFileLoader> jsonFileLoaderMock,
        LoadEntityModuleSystemCommandHandler handler
    )
    {
        var files = new List<StandardFileInfo>
        {
            new(
                "name",
                "directoryInfo",
                "fullName",
                "ext"
            ),
        };
        senderMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    Path.Combine(
                        serverInfoMock.Object.ClientPath,
                        "Modules",
                        "Player"
                    )
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            files
        );

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<EntityScriptModule>(
                "fullName"
            )
        ).ReturnsAsync(
            scriptModule
        );

        // When
        var actual = await handler.Handle(
            new LoadEntityModuleSystemCommand(),
            CancellationToken.None
        );

        // Then
        entityModuleRepositoryMock.Verify(
            mock => mock.AddPlayerModule(
                scriptModule
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task Do_Not_Load_Script_Module_When_Not_Loaded_Successfuly(
        // Given
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<EntityModuleRepository> entityModuleRepositoryMock,
        [Frozen] Mock<IJsonFileLoader> jsonFileLoaderMock,
        LoadEntityModuleSystemCommandHandler handler
    )
    {
        var files = new List<StandardFileInfo>
        {
            new(
                "name",
                "directoryInfo",
                "fullName",
                "ext"
            ),
        };
        senderMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    Path.Combine(
                        serverInfoMock.Object.ClientPath,
                        "Modules",
                        "Player"
                    )
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            files
        );

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<EntityScriptModule>(
                "fullName"
            )
        ).ReturnsAsync(
            default(EntityScriptModule)
        );

        // When
        var actual = await handler.Handle(
            new LoadEntityModuleSystemCommand(),
            CancellationToken.None
        );

        // Then
        entityModuleRepositoryMock.Verify(
            mock => mock.AddPlayerModule(
                It.IsAny<EntityScriptModule>()
            ),
            Times.Never()
        );
        entityModuleRepositoryMock.Verify(
            mock => mock.AddBaseModule(
                It.IsAny<EntityScriptModule>()
            ),
            Times.Never()
        );
    }
}
