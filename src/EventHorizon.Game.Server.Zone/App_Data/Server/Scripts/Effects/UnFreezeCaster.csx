using System.Collections.Generic;
using EventHorizon.Zone.Core.Events.Entity.Movement;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");

// Stop the movement of the Caster Entity on the Server
await Services.Mediator.Publish(
    new EntityCanMoveEvent
    {
        EntityId = caster.Id
    }
);

var freezeActionData = new
{
    Id = caster.Id
};
var freezeAction = new ClientSkillActionEvent
{
    Action = "Actions_UnFreezeEntity.js",
    Data = freezeActionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        freezeAction
    }
};