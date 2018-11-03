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
    Action = "un_freeze_entity",
    Data = freezeActionData
};

return new List<ClientSkillActionEvent>
{
    freezeAction
};