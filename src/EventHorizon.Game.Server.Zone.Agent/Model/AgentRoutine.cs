namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentRoutine
    {
        /// <summary>
        /// Default routine of an Agent, usually means they are not doing anything to note.
        /// </summary>
        public static readonly AgentRoutine IDLE = new AgentRoutine("IDLE");
        /// <summary>
        /// The Agent is moving from one position to another.
        /// </summary>
        public static readonly AgentRoutine MOVE = new AgentRoutine("MOVE");
        /// <summary>
        /// The Agent is looking for its next position to MOVE to.
        /// </summary>
        public static readonly AgentRoutine WANDER = new AgentRoutine("WANDER");
        /// <summary>
        /// The Agent should move away from a target.
        /// </summary>
        public static readonly AgentRoutine FLEE = new AgentRoutine("FLEE");
        /// <summary>
        /// The Agent trying to attack a target.
        /// </summary>
        public static readonly AgentRoutine ATTACK = new AgentRoutine("ATTACK");

        /// <summary>
        /// The Agent is following a Script based routine.
        /// </summary>
        public static readonly AgentRoutine SCRIPT = new AgentRoutine("SCRIPT");

        /// <summary>
        /// The Agent is moving away from a target.
        /// </summary>
        public static readonly AgentRoutine FLEEING = new AgentRoutine("FLEEING");

        private string _type;
        public string Type { get { return _type; } }

        public AgentRoutine(string type)
        {
            _type = type;
        }
    }
}