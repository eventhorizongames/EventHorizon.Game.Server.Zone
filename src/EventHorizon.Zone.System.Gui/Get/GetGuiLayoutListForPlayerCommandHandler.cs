using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.Model;
using EventHorizon.Zone.System.Gui.Events.Layout;
using System.Collections.Generic;

namespace EventHorizon.Zone.System.Gui.Get
{
    public class GetGuiLayoutListForPlayerCommandHandler : IRequestHandler<GetGuiLayoutListForPlayerCommand, IEnumerable<GuiLayout>>
    {
        readonly IMediator _mediator;
        readonly GuiState _guiState;

        public GetGuiLayoutListForPlayerCommandHandler(
            IMediator mediator,
            GuiState guiState
        )
        {
            _mediator = mediator;
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