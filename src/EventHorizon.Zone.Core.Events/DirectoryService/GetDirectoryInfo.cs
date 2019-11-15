using EventHorizon.Zone.Core.Model.DirectoryService;
using MediatR;

namespace EventHorizon.Zone.Core.Events.DirectoryService
{
    public struct GetDirectoryInfo : IRequest<StandardDirectoryInfo>
    {
        public string DirectoryFullName { get; }
        
        public GetDirectoryInfo(
            string directoryFullName
        )
        {
            DirectoryFullName = directoryFullName;
        }
    }
}