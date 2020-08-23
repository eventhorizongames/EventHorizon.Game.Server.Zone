namespace EventHorizon.Zone.System.Client.Scripts.Compile
{
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::System.Threading;
    using MediatR;
    using EventHorizon.Zone.System.Client.Scripts.State;

    public class CompileClientScriptCommandHandler
        : IRequestHandler<CompileClientScriptCommand>
    {
        private readonly ClientScriptRepository _clientScriptRepository;

        public CompileClientScriptCommandHandler(
            ClientScriptRepository clientScriptRepository
        )
        {
            _clientScriptRepository = clientScriptRepository;
        }

        public Task<Unit> Handle(
            CompileClientScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            var scripts = _clientScriptRepository.All().Where(
                a => a.ScriptType == Model.ClientScriptType.CSharp
            );
            if (scripts.Any())
            {

            }
            return Unit.Task;
        }
    }
}
