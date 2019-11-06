using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Server.Core.Register;
using EventHorizon.Game.Server.Zone.Server.Core.State;
using EventHorizon.Game.Server.Zone.Server.Core.Stop;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Core.Check
{
    public class CheckCoreServerConnectionEventHandler : INotificationHandler<CheckCoreServerConnectionEvent>
    {
        private static int MAX_RETRIES = 3;

        readonly IMediator _mediator;
        readonly ServerCoreCheckState _serverCoreCheckState;

        public CheckCoreServerConnectionEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            CheckCoreServerConnectionEvent notification,
            CancellationToken cancellationToken
        )
        {
            var regsitered = await _mediator.Send(
                new QueryForRegistrationWithCoreServer()
            );
            if (!regsitered)
            {
                // Register with the server
                await _mediator.Publish(
                    new RegisterWithCoreServerEvent()
                );
                // Reset Check
                _serverCoreCheckState.Reset();
                return;
            }
            var checksDone = _serverCoreCheckState.TimesChecked();
            if (checksDone >= MAX_RETRIES)
            {
                await _mediator.Publish(
                    new StopCoreServerConnectionEvent()
                );
                await _mediator.Publish(
                    new RegisterWithCoreServerEvent()
                );
                _serverCoreCheckState.Reset();
                return;
            }

            _serverCoreCheckState.Check();

        }
    }
}