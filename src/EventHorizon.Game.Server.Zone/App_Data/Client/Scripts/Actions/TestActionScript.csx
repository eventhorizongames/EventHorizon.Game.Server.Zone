using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Observer.Model;
using EventHorizon.Game.Client.Engine.Events.Testing;
using EventHorizon.Game.Client.Engine.Model.Scripting.Api;
using EventHorizon.Game.Client.Engine.Model.Scripting.Services;
using EventHorizon.Game.Client.Engine.Model.Scripting.Data;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        System.Console.WriteLine("Hello");
        var observer = new __SCRIPT__Observer(
            services,
            data
        );
        var eventList = data.Get<IList<ObserverBase>>("eventList") ?? new List<ObserverBase>();
        eventList.Add(
            observer
        );
        services.RegisterObserver(
            observer
        );
        data.Set(
            "eventList", 
            eventList
        );
    }
}

public class __SCRIPT__Observer
    : ScriptTestingEventObserver
{
    private readonly ScriptServices _scriptServices;
    private readonly ScriptData _scriptData;

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data
    )
    {
        _scriptServices = services;
        _scriptData = data;
    }

    public Task Handle(
        ScriptTestingEvent notification
    )
    {
        System.Console.WriteLine("__SCRIPT__Observer from Script Triggered ");
        return Task.CompletedTask;
    }
}
