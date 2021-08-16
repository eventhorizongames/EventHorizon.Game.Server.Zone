using MediatR;

namespace EventHorizon.Zone.Core.Events.FileService
{
    public struct CreateFile : IRequest<bool>
    {
        public string FileFullName { get; }

        public CreateFile(
            string fileFullName
        )
        {
            FileFullName = fileFullName;
        }
    }
}
