namespace EventHorizon.Zone.Core.DirectoryService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;

    using MediatR;

    public class IsDirectoryEmptyHandler
        : IRequestHandler<IsDirectoryEmpty, bool>
    {
        private readonly DirectoryResolver _directoryResolver;

        public IsDirectoryEmptyHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<bool> Handle(
            IsDirectoryEmpty request,
            CancellationToken cancellationToken
        ) => _directoryResolver.IsEmpty(
            request.DirectoryFullName
        ).FromResult();
    }
}
