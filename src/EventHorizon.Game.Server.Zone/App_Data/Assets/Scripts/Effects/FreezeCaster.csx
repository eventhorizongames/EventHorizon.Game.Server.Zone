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
    Action = "freeze_entity",
    Data = freezeActionData
};

return new List<ClientSkillActionEvent>
{
    freezeAction
};