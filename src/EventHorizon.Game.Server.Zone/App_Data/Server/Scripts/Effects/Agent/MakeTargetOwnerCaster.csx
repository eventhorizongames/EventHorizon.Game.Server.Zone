/// <summary>
/// Effect Id: make_target_owner_caster
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// Data: { mesageTemplateKey: string; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");

var casterGlobalId = caster.GlobalId;
var targetOwner = target.GetProperty<dynamic>("ownerState");

targetOwner["ownerId"] = casterGlobalId;
targetOwner["canBeCaptured"] = false;

await Services.Mediator.Publish(
    new ClientActionEntityClientChangedToAllEvent
    {
        Data = new EntityChangedData(
            target
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