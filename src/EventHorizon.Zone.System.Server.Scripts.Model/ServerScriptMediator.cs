﻿namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public interface ServerScriptMediator
    {
        Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default
        );

        Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default
        ) where TNotification : INotification;
    }
}
