namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    using global::System.Collections.Generic;
    using global::System.Text;

    public class SerializedBehaviorNode
    {
        public string Type { get; set; } = "ACTION";
        public string? Status { get; set; }
        public string Fire { get; set; } = string.Empty;
        public int FailGate { get; set; }
        public bool Reset { get; set; }
        public IList<SerializedBehaviorNode>? NodeList { get; set; }

        private int _token;
        public int GetToken(
            int root
        )
        {
            if (_token != 0)
            {
                return _token;
            }

            var tokenString = new StringBuilder(
                string.Empty
            ).Append(
                root
            ).Append(
                Type
            ).Append(
                Status ?? string.Empty
            ).Append(
                Fire
            ).Append(
                FailGate
            ).Append(
                Reset
            );

            if (NodeList.IsNotNull())
            {
                foreach (var node in NodeList)
                {
                    // We want to make sure all leaf nodes do not match other leaf nodes that are the same.
                    var childRoot = root++;
                    tokenString.Append(
                        node.GetToken(
                            childRoot
                        )
                    );
                }
            }

            _token = tokenString.ToString()
                .GetDeterministicHashCode();

            return _token;
        }
    }
}
