/// <summary>
/// Effect Id: make_target_owner_caster
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// Data: { mesageTemplateKey: string; }
/// </summary>


var casterGlobalId = Caster.GlobalId;
var targetOwner = Target.GetProperty<dynamic>("ownerState");

targetOwner["ownerId"] = casterGlobalId;

// TODO: Add Client Skill Action that will update all clients with new Owner on Target
return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>()
};