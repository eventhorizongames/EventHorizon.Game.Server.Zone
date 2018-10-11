using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Gui.Events;
using MediatR;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Events.Gui;

namespace EventHorizon.Game.Server.Zone.Gui.Get
{
    public class GetGuiLayoutForPlayerHandler : IRequestHandler<GetGuiLayoutForPlayerEvent, GuiLayout>
    {
        readonly IMediator _mediator;
        readonly GuiState _guiState;
        public GetGuiLayoutForPlayerHandler(
            IMediator mediator,
            GuiState guiState)
        {
            _mediator = mediator;
            _guiState = guiState;
        }
        public async Task<GuiLayout> Handle(GetGuiLayoutForPlayerEvent request, CancellationToken cancellationToken)
        {
            var dataList = new List<GuiControlData>();
            await _mediator.Publish(
                new FillGuiDataListForPlayerEvent
                {
                    Player = request.Player,
                    DataListRef = dataList
                }
            );
            return new GuiLayout
            {
                TemplateList = _guiState.All(),
                LayoutList = _guiState.AllLayouts(),
                DataList = dataList
            };
        }
    }
}