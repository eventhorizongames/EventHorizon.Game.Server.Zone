var animation = (string) Data["animation"];

var actionData = new
{
    EntityId = Caster.Id,
    DirectionEntityId = Target.Id,
    Animation = animation
};
var action = new ClientSkillActionEvent
{
    Action = "start_entity_direction_animation",
    Data = actionData
};

return new List<ClientSkillActionEvent>
{
    action
};