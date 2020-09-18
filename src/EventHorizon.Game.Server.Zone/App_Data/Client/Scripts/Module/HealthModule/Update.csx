/*
data:
    entity: IObjectEntity
    guiId: string
    observer: ObserverBase
    updateObserver: ObserverBase
*/

using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        return Task.CompletedTask;
    }
}
