namespace EventHorizon.Zone.System.Agent.Model
{
    public struct AgentBehavior
    {
        public static readonly string PROPERTY_NAME = "behavior";

        public string TreeId { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "treeId":
                        return this.TreeId;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "treeId":
                        this.TreeId = (string)value;
                        break;

                    default:
                        break;
                }
            }
        }
        public static readonly AgentBehavior NEW = new AgentBehavior
        {
            TreeId = "DEFAULT"
        };
    }
}