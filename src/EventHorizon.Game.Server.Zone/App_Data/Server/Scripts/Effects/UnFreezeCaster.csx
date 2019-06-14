// Stop the movement of the Caster Entity on the Server
Services.Mediator.Publish(
    new EntityCanMoveEvent
    {
        EntityId = Caster.Id
    }
);

var freezeActionData = new
{
    Id = Caster.Id
};
var freezeAction = new ClientSkillActionEvent
{
    Action = "Actions_UnFreezeEntity.js",
    Data = freezeActionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        freezeAction
    }
};