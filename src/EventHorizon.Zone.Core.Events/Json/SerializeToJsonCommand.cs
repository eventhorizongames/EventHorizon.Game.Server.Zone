namespace EventHorizon.Zone.Core.Events.Json;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record SerializeToJsonCommand(
    object ObjectToSerialize
) : IRequest<CommandResult<SerializeToJsonResult>>;

public record SerializeToJsonResult(
    string Json
);
