var animation = (string) Data["animation"];

var actionData = new
{
    EntityId = Caster.Id,
    DirectionEntityId = Target.Id,
    Animation = animation
};
var action = new ClientSkillActionEvent
{
    Action = "TODO: TODO: TODO: ",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};