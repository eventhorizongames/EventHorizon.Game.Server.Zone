namespace EventHorizon.Zone.Core.Lifetime
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;

    public class RunServerStartupCommandHandler : IRequestHandler<RunServerStartupCommand>
    {
        private readonly IMediator _mediator;

        public RunServerStartupCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(
            RunServerStartupCommand request,
            CancellationToken cancellationToken
        )
        {
            var onServerStartupCommandList = AppDomain
                .CurrentDomain.GetAssemblies()
                .SelectMany(x =>
                {
                    try
                    {
                        return x.DefinedTypes;
                    }
                    catch
                    {
                        return Array.Empty<TypeInfo>();
                    }
                })
                .Where(
                    type =>
                        !type.IsInterface && typeof(OnServerStartupCommand).IsAssignableFrom(type)
                );

            foreach (var onServerServerCommand in onServerStartupCommandList)
            {
                try
                {
                    if (
                        Activator.CreateInstance(onServerServerCommand)
                        is not OnServerStartupCommand command
                    )
                    {
                        throw new SystemException(
                            $"Failed '{onServerServerCommand.Name}' is not a {typeof(OnServerStartupCommand)}"
                        );
                    }

                    var result = await _mediator.Send(command, cancellationToken);

                    if (!result.Success)
                    {
                        throw new SystemException(
                            $"Failed '{onServerServerCommand.Name}' with ErrorCode: {result.ErrorCode}"
                        );
                    }
                }
                catch (SystemException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new SystemException(
                        $"Failed '{onServerServerCommand.Name}' with Exception.",
                        ex
                    );
                }
            }

        }
    }
}
