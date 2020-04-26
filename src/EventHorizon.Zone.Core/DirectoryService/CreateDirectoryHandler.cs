namespace EventHorizon.Zone.Core.DirectoryService
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateDirectoryHandler : IRequestHandler<CreateDirectory, bool>
    {
        private readonly DirectoryResolver _directoryResolver;

        public CreateDirectoryHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<bool> Handle(
            CreateDirectory request,
            CancellationToken cancellationToken
        ) => _directoryResolver.CreateDirectory(
            request.DirectoryFullName
        ).FromResult();
    }
}