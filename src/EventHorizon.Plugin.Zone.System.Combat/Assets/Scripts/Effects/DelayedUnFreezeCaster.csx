var actionData = new
{
    Id = Caster.Id
};

var action = new ClientSkillActionEvent
{
    Delay = (long) Data["delay"],
    Action = "un_freeze_entity",
    Data = actionData
};

return new List<ClientSkillActionEvent>
{
    action
};