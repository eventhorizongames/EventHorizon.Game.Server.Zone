namespace EventHorizon.Zone.System.Editor.ExternalHub
{
    using EventHorizon.Zone.System.Editor.Events.Create;
    using EventHorizon.Zone.System.Editor.Events.Delete;
    using EventHorizon.Zone.System.Editor.Events.Save;
    using EventHorizon.Zone.System.Editor.Events.State;
    using EventHorizon.Zone.System.Editor.Model;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize("UserIdOrAdmin")]
    public class SystemEditorHub : Hub
    {
        readonly IMediator _mediator;
        public SystemEditorHub(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        //public override Task OnConnectedAsync()
        //{
        //    if (!Context.User.IsInRole("Admin"))
        //    {
        //        throw new Exception("no_role");
        //    }
        //    return Task.CompletedTask;
        //}

        public Task<IEditorNodeList> GetEditorState()
        {
            return _mediator.Send(
                new GetEditorState()
            );
        }

        public Task<StandardEditorFile> GetEditorFileContent(
            IList<string> path,
            string fileName
        )
        {
            return _mediator.Send(
                new GetEditorFileContent(
                    path,
                    fileName
                )
            );
        }
        public Task<EditorResponse> SaveEditorFileContent(
            IList<string> path,
            string fileName,
            string content
        )
        {
            return _mediator.Send(
                new SaveEditorFileContent(
                    path,
                    fileName,
                    content
                )
            );
        }
        public Task<EditorResponse> CreateEditorFile(
            IList<string> path,
            string fileName
        )
        {
            return _mediator.Send(
                new CreateEditorFile(
                    path,
                    fileName
                )
            );
        }
        public Task<EditorResponse> CreateEditorFolder(
            IList<string> path,
            string folderName
        )
        {
            return _mediator.Send(
                new CreateEditorFolder(
                    path,
                    folderName
                )
            );
        }
        public Task<EditorResponse> DeleteEditorFile(
            IList<string> path,
            string fileName
        )
        {
            return _mediator.Send(
                new DeleteEditorFile(
                    path,
                    fileName
                )
            );
        }
        public Task<EditorResponse> DeleteEditorFolder(
            IList<string> path,
            string folderName
        )
        {
            return _mediator.Send(
                new DeleteEditorFolder(
                    path,
                    folderName
                )
            );
        }
    }
}
