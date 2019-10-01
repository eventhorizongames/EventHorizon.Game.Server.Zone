using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
using EventHorizon.Zone.System.Agent.Save.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Zone.System.Agent.Save
{
    public class SaveAgentStateScheduledTask : IScheduledTask
    {
        // CRON for every 5 seconds
        public string Schedule => "*/5 * * * * *";
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SaveAgentStateScheduledTask(
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(
            CancellationToken cancellationToken
        )
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                await serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(
                        new SaveAgentStateEvent()
                    );
            }
        }
    }
}