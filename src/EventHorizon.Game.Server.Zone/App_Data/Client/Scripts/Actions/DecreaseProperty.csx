/*
data:
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Entity.Tag;
using EventHorizon.Game.Client.Engine.Entity.Tracking.Query;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using EventHorizon.Game.Client.Engine.Systems.Entity.Model;
using EventHorizon.Game.Client.Systems.Entity.Changed;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Script");
        var entities = await services.Mediator.Send(
            new QueryForEntity(
                TagBuilder.CreateEntityIdTag(
                    data.Get<long>("id").ToString()
                )
            )
        );

        if (!entities.Success
            || entities.Result.Count() <= 0)
        {
            return;
        }

        var entity = entities.Result.First();
        var propertyName = data.Get<string>("propertyName");
        var valueProperty = data.Get<string>("valueProperty");
        var amount = data.Get<int>("amount");

        logger.LogDebug(
            "Decreasing Property: PropertyName: {PropertyName} | ValueProperty: {ValueProperty} | Amount: {Amount}",
            propertyName,
            valueProperty,
            amount
        );

        var property = entity.GetProperty<StandardObjectProperty>(
            data.Get<string>("propertyName")
        );
        var propertyValue = property[valueProperty].Cast<decimal>();
        propertyValue = propertyValue - amount;
        property[valueProperty] = propertyValue;
        entity.SetProperty(
            propertyName,
            property
        );

        await services.Mediator.Publish(
            new EntityChangedSuccessfullyEvent(
                entity.EntityId
            )
        );
    }
}
