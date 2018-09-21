namespace EventHorizon.Game.Server.Zone.Agent.Ai
{
    public enum AiRoutine
    {
        /// <summary>
        /// Default routine of an Agent, usually means they are not doing anything to note.
        /// </summary>
        IDLE = 0,
        /// <summary>
        /// The Agent is moving from one position to another.
        /// </summary>
        MOVE = 1,
        /// <summary>
        /// The Agent is looking for its next position to MOVE to.
        /// </summary>
        WANDER = 2,
        /// <summary>
        /// The Agent should move away from a target.
        /// </summary>
        FLEE = 3,
        /// <summary>
        /// The Agent trying to attack a target.
        /// </summary>
        ATTACK = 4,

        /// <summary>
        /// The Agent is following a Script based routine.
        /// </summary>
        SCRIPT = 5,

        /// <summary>
        /// The Agent is moving away from a target.
        /// </summary>
        FLEEING = 6,

    }
}