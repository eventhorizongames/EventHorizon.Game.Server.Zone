namespace Microsoft.AspNetCore.Builder
{
    using System;
    using System.Threading;

    using MediatR;

    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IServiceScope CreateServiceScope(
            this IApplicationBuilder applicationBuilder
        )
        {
            var factory = applicationBuilder
                   .ApplicationServices
                   .GetService<IServiceScopeFactory>();
            if (factory == null)
            {
                throw new InvalidOperationException(
                    $"{typeof(IServiceScopeFactory)} was not found"
                );
            }

            return factory.CreateScope();
        }

        public static IApplicationBuilder SendMediatorCommand(
            this IApplicationBuilder applicationBuilder,
            IRequest command
        )
        {
            using var scope = applicationBuilder.CreateServiceScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            if (mediator == null)
            {
                throw new InvalidOperationException(
                    $"{typeof(IMediator)} was not found"
                );
            }
            else if (command == null)
            {
                throw new InvalidOperationException(
                    $"command was null"
                );
            }

            mediator.Send(
                command,
                CancellationToken.None
            ).ConfigureAwait(true).GetAwaiter().GetResult();

            return applicationBuilder;
        }

        public static IApplicationBuilder SendMediatorCommand<T, J>(
            this IApplicationBuilder applicationBuilder,
            T command
        ) where T : IRequest<J>
        {
            using var scope = applicationBuilder.CreateServiceScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            if (mediator == null)
            {
                throw new InvalidOperationException(
                    $"{typeof(IMediator)} was not found"
                );
            }
            else if (command == null)
            {
                throw new InvalidOperationException(
                    $"{typeof(T)} was null command"
                );
            }

            mediator.Send(
                command,
                CancellationToken.None
            ).ConfigureAwait(true).GetAwaiter().GetResult();

            return applicationBuilder;
        }
    }
}
