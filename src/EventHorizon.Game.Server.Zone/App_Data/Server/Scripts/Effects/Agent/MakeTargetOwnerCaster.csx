/// <summary>
/// Effect Id: make_target_owner_caster
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// Data: { mesageTemplateKey: string; }
/// </summary>

using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client.DataType;

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
    Action = "Actions_Agent_AgentCaptured.js"
};
return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};