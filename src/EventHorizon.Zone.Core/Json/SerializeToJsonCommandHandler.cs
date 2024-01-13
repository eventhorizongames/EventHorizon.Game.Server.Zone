namespace EventHorizon.Zone.Core.Json;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Api;
using EventHorizon.Zone.Core.Events.Json;
using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public class SerializeToJsonCommandHandler
    : IRequestHandler<SerializeToJsonCommand, CommandResult<SerializeToJsonResult>>
{
    private readonly SerializeToJsonService _service;

    public SerializeToJsonCommandHandler(
        SerializeToJsonService service
    )
    {
        _service = service;
    }

    public Task<CommandResult<SerializeToJsonResult>> Handle(
        SerializeToJsonCommand request,
        CancellationToken cancellationToken
    )
    {
        return new CommandResult<SerializeToJsonResult>(
            new SerializeToJsonResult(
                _service.Serialize(
                    request.ObjectToSerialize
                )
            )
        ).FromResult();
    }
}
