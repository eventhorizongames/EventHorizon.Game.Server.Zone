namespace EventHorizon.Zone.Core.Events.FileService
{
    using EventHorizon.Zone.Core.Model.FileService;

    using MediatR;

    public struct GetFileInfo : IRequest<StandardFileInfo>
    {
        public string FileFullName { get; }

        public GetFileInfo(
            string fileFullName
        )
        {
            FileFullName = fileFullName;
        }
    }
}
