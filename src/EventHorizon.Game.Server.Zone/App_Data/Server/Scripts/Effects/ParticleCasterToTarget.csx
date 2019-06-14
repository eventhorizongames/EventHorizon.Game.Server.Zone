var actionData = new
{
    StartEntityId = Caster.Id,
    EndingEntityId = Target.Id,
    ParticleTemplateId = (string) Data["particleTemplateId"],
    Duration = (long) Data["duration"],
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_ParticleToEntity.js",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};