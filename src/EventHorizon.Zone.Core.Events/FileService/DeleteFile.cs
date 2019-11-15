using MediatR;

namespace EventHorizon.Zone.Core.Events.FileService
{
    public struct DeleteFile : IRequest
    {
        public string FileFullName { get; }

        public DeleteFile(
            string fileFullName
        )
        {
            FileFullName = fileFullName;
        }
    }
}