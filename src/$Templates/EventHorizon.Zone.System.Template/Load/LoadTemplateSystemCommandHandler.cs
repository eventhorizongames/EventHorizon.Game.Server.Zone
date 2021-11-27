namespace EventHorizon.Zone.System.Template.Load;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class LoadTemplateSystemCommandHandler
    : IRequestHandler<LoadTemplateSystemCommand, LoadTemplateSystemResult>
{
    public Task<LoadTemplateSystemResult> Handle(
        LoadTemplateSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        return new LoadTemplateSystemResult(
            true,
            new List<string>().Where(
                reason => reason.IsNotNullOrEmpty()
            ).ToArray()
        ).FromResult();
    }
}
