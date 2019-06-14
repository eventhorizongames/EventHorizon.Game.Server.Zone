// Stop the movement of the Caster Entity on the Server
Services.Mediator.Publish(
    new StopEntityMovementEvent
    {
        EntityId = Caster.Id
    }
);

// Create Client action.
var freezeActionData = new
{
    Id = Caster.Id
};
var freezeAction = new ClientSkillActionEvent
{
    Action = "Actions_FreezeEntity.js",
    Data = freezeActionData
};

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>
    {
        freezeAction
    }
};