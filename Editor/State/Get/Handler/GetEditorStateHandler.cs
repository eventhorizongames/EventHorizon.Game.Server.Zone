using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Load;
using EventHorizon.Game.Server.Zone.Editor.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace EventHorizon.Game.Server.Zone.Editor.State.Get.Handler
{
    public class GetEditorStateHandler : IRequestHandler<GetEditorStateEvent, EditorState>
    {
        readonly IMediator _mediator;
        public GetEditorStateHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<EditorState> Handle(GetEditorStateEvent request, CancellationToken cancellationToken)
        {
            return new EditorState
            {
                Map = await _mediator.Send(new LoadEditorZoneMapEvent()),
                AssetList = await _mediator.Send(new LoadEditorAssetListEvent()),
                EntityList = await _mediator.Send(new LoadEditorEntityListEvent()),
                ScriptList = await _mediator.Send(new LoadEditorScriptListEvent()),
            };
        }
    }
}