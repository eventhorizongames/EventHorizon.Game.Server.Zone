var actionData = new
{
    Id = Caster.Id
};

var action = new ClientSkillActionEvent
{
    Delay = (Int64) Data["delay"],
    Action = "un_freeze_entity",
    Data = actionData
};

return new List<ClientSkillActionEvent>
{
    action
};