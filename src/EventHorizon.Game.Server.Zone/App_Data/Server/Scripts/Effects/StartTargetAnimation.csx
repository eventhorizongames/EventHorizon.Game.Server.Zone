var animation = Data["animation"];

var actionData = new
{
    EntityId = Target.Id,
    Animation = animation
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_StartEntityAnimation.js",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};