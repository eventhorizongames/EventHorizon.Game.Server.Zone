namespace EventHorizon.Game.Server.Zone.Agent.Ai
{
    public class AiRoutines
    {
        /// <summary>
        /// Default routine of an Agent, usually means they are not doing anything to note.
        /// </summary>
        public static readonly string IDLE = "IDLE";
        /// <summary>
        /// The Agent is moving from one position to another.
        /// </summary>
        public static readonly string MOVE = "MOVE";
        /// <summary>
        /// The Agent is looking for its next position to MOVE to.
        /// </summary>
        public static readonly string WANDER = "WANDER";
        /// <summary>
        /// The Agent is moving away from a target.
        /// </summary>
        public static readonly string FLEE = "FLEE";
        /// <summary>
        /// The Agent trying to attack a target.
        /// </summary>
        public static readonly string ATTACK = "ATTACK";

        /// <summary>
        /// The Agent is following a Script based routine.
        /// </summary>
        public static readonly string SCRIPT = "SCRIPT";
    }
}