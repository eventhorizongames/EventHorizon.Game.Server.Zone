namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    public interface ClientSkillAction
    {
        string Action { get; }
        object Data { get; }
    }
}
