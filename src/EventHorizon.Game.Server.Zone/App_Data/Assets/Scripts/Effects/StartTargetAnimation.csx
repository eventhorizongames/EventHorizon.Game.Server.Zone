var animation = Data["animation"];

var actionData = new
{
    EntityId = Target.Id,
    Animation = animation
};
var action = new ClientSkillActionEvent
{
    Action = "start_entity_animation",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};