/// <summary>
/// Effect Id: make_target_owner_caster
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// Data: { mesageTemplateKey: string; }
/// </summary>

using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Client.DataType;

var casterGlobalId = Caster.GlobalId;
var targetOwner = Target.GetProperty<dynamic>("ownerState");

targetOwner["ownerId"] = casterGlobalId;
targetOwner["canBeCaptured"] = false;

await Services.Mediator.Publish(
    new ClientActionEntityClientChangedToAllEvent
    {
        Data = new EntityChangedData(
            Target
        )
    }
);

var action = new ClientSkillActionEvent
{
    Action = "agent_captured"
};
return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};