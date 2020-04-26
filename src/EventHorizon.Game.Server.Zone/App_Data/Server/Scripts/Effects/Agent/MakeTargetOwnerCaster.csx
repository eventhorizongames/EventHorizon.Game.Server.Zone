/// <summary>
/// Effect Id: make_target_owner_caster
/// 
/// Caster: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />  
/// Target: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Data: { mesageTemplateKey: string; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Update;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");

var casterGlobalId = caster.GlobalId;

var casterCompanionManagement = caster.GetProperty<CompanionManagementState>(CompanionManagementState.PROPERTY_NAME);
var targetOwner = target.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);

targetOwner.OwnerId = casterGlobalId;
targetOwner.CanBeCaptured = false;

target.SetProperty(
    OwnerState.PROPERTY_NAME,
    targetOwner
);

await Services.Mediator.Send(
    new UpdateEntityCommand(
        AgentAction.PROPERTY_CHANGED,
        target
    )
);

var wasChanged = await Services.Mediator.Send(
    new ChangeActorBehaviorTreeCommand(
        target,
        casterCompanionManagement.CapturedBehaviorTreeId
    )
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