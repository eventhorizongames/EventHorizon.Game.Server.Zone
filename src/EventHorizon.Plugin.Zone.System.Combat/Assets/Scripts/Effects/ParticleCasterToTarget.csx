var particleTemplateId = (string) Data["particleTemplateId"];

var actionData = new
{
    StartEntityId = Caster.Id,
    EndingEntityId = Target.Id,
    ParticleTemplateId = particleTemplateId
};
var action = new ClientSkillActionEvent
{
    Duration = (long) Data["duration"],
    Action = "particle_to_entity",
    Data = actionData
};

return new List<ClientSkillActionEvent>
{
    action
};