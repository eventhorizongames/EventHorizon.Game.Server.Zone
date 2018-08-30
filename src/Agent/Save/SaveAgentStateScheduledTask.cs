using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Agent.Save
{
    public class SaveAgentStateScheduledTask : IScheduledTask
    {
        public string Schedule => "*/5 * * * * *"; // Every 5 seconds
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SaveAgentStateScheduledTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                await serviceScope.ServiceProvider.GetService<IMediator>().Publish(new SaveAgentStateEvent());
            }
        }
    }
}