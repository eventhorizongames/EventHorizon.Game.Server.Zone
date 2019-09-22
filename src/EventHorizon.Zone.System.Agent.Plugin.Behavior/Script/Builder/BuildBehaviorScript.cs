using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Builder
{
    public struct BuildBehaviorScript : IRequest<BehaviorScript>
    {
        public string Id { get; set; }
        public string Content { get; set; }

        public BuildBehaviorScript(
            string id,
            string content
        )
        {
            Id = id;
            Content = content;
        }

        public class BuildBehaviorScriptHandler : IRequestHandler<BuildBehaviorScript, BehaviorScript>
        {
            public Task<BehaviorScript> Handle(
                BuildBehaviorScript request,
                CancellationToken cancellationToken
            )
            {
                return Task.FromResult(
                    BehaviorScript.CreateScript(
                        request.Id,
                        request.Content
                    )
                );
            }
        }
    }
}