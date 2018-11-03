var actionData = new
{
    StartEntityId = Caster.Id,
    EndingEntityId = Target.Id,
    ParticleTemplateId = (string) Data["particleTemplateId"],
    Duration = (long) Data["duration"],
};
var action = new ClientSkillActionEvent
{
    Action = "particle_to_entity",
    Data = actionData
};

return new List<ClientSkillActionEvent>
{
    action
};