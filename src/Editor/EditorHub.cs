using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.Editor.State.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Editor
{
    [Authorize]
    public class EditorHub : Hub
    {
        readonly IMediator _mediator;
        public EditorHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            if (!Context.User.IsInRole("Admin"))
            {
                throw new System.Exception("no_role");
            }
            return Task.CompletedTask;
        }

        public async Task<EditorState> StateOfEditor()
        {
            return await _mediator.Send(new GetEditorStateEvent());
        }
    }
}