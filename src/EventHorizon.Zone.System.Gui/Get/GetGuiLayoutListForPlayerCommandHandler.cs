namespace EventHorizon.Zone.System.Gui.Get
{
    using EventHorizon.Zone.System.Gui.Api;
    using EventHorizon.Zone.System.Gui.Events.Layout;
    using EventHorizon.Zone.System.Gui.Model;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class GetGuiLayoutListForPlayerCommandHandler : IRequestHandler<GetGuiLayoutListForPlayerCommand, IEnumerable<GuiLayout>>
    {
        private readonly GuiState _guiState;

        public GetGuiLayoutListForPlayerCommandHandler(
            GuiState guiState
        )
        {
            _guiState = guiState;
        }

        public Task<IEnumerable<GuiLayout>> Handle(
            GetGuiLayoutListForPlayerCommand request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _guiState.All()
            );
        }
    }
}
