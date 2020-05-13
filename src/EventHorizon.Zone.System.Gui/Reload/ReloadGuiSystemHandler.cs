namespace EventHorizon.Zone.System.Gui.Reload
{
    using EventHorizon.Zone.System.Gui.Actions.Reload;
    using EventHorizon.Zone.System.Gui.Events.Layout;
    using EventHorizon.Zone.System.Gui.Load;
    using EventHorizon.Zone.System.Gui.Model.Client;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class ReloadGuiSystemHandler : IRequestHandler<ReloadGuiSystem>
    {
        private readonly IMediator _mediator;

        public ReloadGuiSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            ReloadGuiSystem request, 
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new LoadSystemGuiCommand(),
                cancellationToken
            );

            await _mediator.Publish(
                GuiSystemReloadedClientActionToAllEvent.Create(
                    new GuiSystemReloadedClientActionData(
                        await _mediator.Send(
                            new GetGuiLayoutListForPlayerCommand(
                                default
                            )
                        )
                    )
                )
            );

            return Unit.Value;
        }
    }
}
