namespace EventHorizon.Zone.System.Player.Plugin.Action.Register;

using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;
using EventHorizon.Zone.System.Player.Plugin.Action.State;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

public class RegisterPlayerActionHandler : IRequestHandler<RegisterPlayerAction>
{
    readonly ILogger _logger;
    readonly PlayerActionRepository _actionRepository;

    public RegisterPlayerActionHandler(
        ILogger<RegisterPlayerActionHandler> logger,
        PlayerActionRepository actionRepository
    )
    {
        _logger = logger;
        _actionRepository = actionRepository;
    }

    public Task Handle(RegisterPlayerAction request, CancellationToken cancellationToken)
    {
        var entity = new PlayerActionEntity(
            request.Id,
            request.ActionName,
            request.ActionEvent
        );
        try
        {
            _actionRepository.On(entity);
        }
        catch (AlreadyContainsPlayerAction ex)
        {
            _logger.LogError(
                ex,
                "Action Repository already contains a copy of Player Action: \n | Id: {PlayerActionId} \n | Name: {PlayerActionName}",
                request.Id,
                request.ActionName
            );
        }

        return Task.CompletedTask;
    }
}
