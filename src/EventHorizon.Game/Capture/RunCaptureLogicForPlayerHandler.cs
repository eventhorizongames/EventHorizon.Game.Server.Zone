namespace EventHorizon.Game.Capture
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Capture.Logic;
    using EventHorizon.Game.Model;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Player.Events.Find;

    using MediatR;

    public class RunCaptureLogicForPlayerHandler : IRequestHandler<RunCaptureLogicForPlayer>
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTime;

        public RunCaptureLogicForPlayerHandler(
            IMediator mediator,
            IDateTimeService dateTime
        )
        {
            _mediator = mediator;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(
            RunCaptureLogicForPlayer request,
            CancellationToken cancellationToken
        )
        {
            var player = await _mediator.Send(
                new FindPlayerByEntityId(
                    request.PlayerEntityId
                ),
                cancellationToken
            );
            var captureState = player.GetProperty<GamePlayerCaptureState>(
                GamePlayerCaptureState.PROPERTY_NAME
            );
            var escapeCaptureTime = captureState.EscapeCaptureTime;
            var captures = captureState.Captures;

            if (captures == 0)
            {
                return Unit.Value;
            }

            if (ShouldProcessTenSecondMessage(
                captureState,
                escapeCaptureTime
            ))
            {
                await _mediator.Send(
                    new ProcessTenSecondCaptureLogic(
                        player
                    ),
                    cancellationToken
                );
            }

            if (ShouldProcessFiveSecondMessage(
                captureState,
                escapeCaptureTime
            ))
            {
                await _mediator.Send(
                    new ProcessFiveSecondCaptureLogic(
                        player
                    ),
                    cancellationToken
                );
            }

            if (ShouldTriggerEscapeOfCaptures(
                escapeCaptureTime
            ))
            {
                await _mediator.Send(
                    new RunEscapeOfCaptures(
                        player
                    ),
                    cancellationToken
                );
            }

            return Unit.Value;
        }

        private bool ShouldProcessTenSecondMessage(
            GamePlayerCaptureState captureState,
            DateTime escapeCaptureTime
        )
        {
            return escapeCaptureTime.CompareTo(
                _dateTime.Now.AddSeconds(10) // Within 10 Seconds
            ) <= 0 && !captureState.ShownTenSecondMessage;
        }

        private bool ShouldProcessFiveSecondMessage(
            GamePlayerCaptureState captureState,
            DateTime escapeCaptureTime
        )
        {
            return escapeCaptureTime.CompareTo(
                _dateTime.Now.AddSeconds(5) // Within 5 Seconds
            ) <= 0 && !captureState.ShownFiveSecondMessage;
        }

        private bool ShouldTriggerEscapeOfCaptures(
            DateTime escapeCaptureTime
        )
        {
            return escapeCaptureTime.CompareTo(
                _dateTime.Now
            ) <= 0;
        }
    }
}
