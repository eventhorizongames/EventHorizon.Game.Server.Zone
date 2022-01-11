namespace EventHorizon.Zone.System.ClientEntities.Save;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Register;
using EventHorizon.Zone.System.ClientEntities.State;
using EventHorizon.Zone.System.ClientEntities.Unregister;

using global::System;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class SaveClientEntityCommandHandler
    : IRequestHandler<SaveClientEntityCommand, SaveClientEntityResponse>
{
    private readonly ISender _sender;
    private readonly ServerInfo _serverInfo;
    private readonly IJsonFileSaver _fileSaver;
    private readonly ClientEntityRepository _repository;

    public SaveClientEntityCommandHandler(
        ISender sender,
        ServerInfo serverInfo,
        IJsonFileSaver fileSaver,
        ClientEntityRepository repository
    )
    {
        _sender = sender;
        _serverInfo = serverInfo;
        _fileSaver = fileSaver;
        _repository = repository;
    }

    public async Task<SaveClientEntityResponse> Handle(
        SaveClientEntityCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            request.ClientEntity.Data.Clear();
            if (!request.ClientEntity.RawData.TryGetValue(
                ClientEntityConstants.METADATA_FILE_FULL_NAME,
                out var fileFullNameFound
            ) || fileFullNameFound is not string fileFullName)
            {
                fileFullName = Path.Combine(
                    _serverInfo.ClientEntityPath,
                    $"{request.ClientEntity.ClientEntityId}.json"
                );
            }

            var fileInfo = await _sender.Send(
                new GetFileInfo(
                    fileFullName
                ),
                cancellationToken
            );
            if (await _sender.Send(
                new DoesFileExist(
                    fileInfo.FullName
                ),
                cancellationToken
            ))
            {
                await _sender.Send(
                    new CreateBackupOfFileContentCommand(
                        new string[] { "Client", "Entity" },
                        fileInfo.Name,
                        await _sender.Send(
                            new ReadAllTextFromFile(
                                fileInfo.FullName
                            )
                        )
                    ),
                    cancellationToken
                );
            }

            await _fileSaver.SaveToFile(
                fileInfo.DirectoryName,
                fileInfo.Name,
                ClientEntityFromEntityToDetails.Map(
                    request.ClientEntity
                )
            );

            await _sender.Send(
                new UnregisterClientEntity(
                    request.ClientEntity.GlobalId
                ),
                cancellationToken
            );

            await _sender.Send(
                new RegisterClientEntityCommand(
                    request.ClientEntity
                ),
                cancellationToken
            );

            var registeredClientEntity = _repository.Find(
                request.ClientEntity.GlobalId
            );

            return new SaveClientEntityResponse(
                true,
                registeredClientEntity
            );
        }
        catch (Exception)
        {
            return new SaveClientEntityResponse(
                "exception"
            );
        }
    }
}
