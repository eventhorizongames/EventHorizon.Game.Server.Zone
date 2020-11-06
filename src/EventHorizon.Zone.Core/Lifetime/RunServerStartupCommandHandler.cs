namespace EventHorizon.Zone.Core.Lifetime
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;

    public class RunServerStartupCommandHandler
        : IRequestHandler<RunServerStartupCommand>
    {
        private readonly IMediator _mediator;

        public RunServerStartupCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            RunServerStartupCommand request,
            CancellationToken cancellationToken
        )
        {
            var onServerStartupCommandList = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(type => !type.IsInterface && typeof(OnServerStartupCommand).IsAssignableFrom(type));

            foreach (var onServerServerCommand in onServerStartupCommandList)
            {
                var result = await _mediator.Send(
                    Activator.CreateInstance(
                        onServerServerCommand
                    ) as OnServerStartupCommand,
                    cancellationToken
                );

                if (!result.Success)
                {
                    throw new SystemException(
                        $"Failed '{onServerServerCommand.Name}' with ErrorCode: {result.ErrorCode}"
                    );
                }
            }

            return Unit.Value;
        }
    }
}
