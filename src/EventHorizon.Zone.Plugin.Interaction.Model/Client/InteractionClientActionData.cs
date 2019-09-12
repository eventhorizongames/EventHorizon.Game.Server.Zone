using EventHorizon.Game.Server.Zone.Model.Client;

namespace EventHorizon.Zone.Plugin.Interaction.Model.Client
{
    public struct InteractionClientActionData : IClientActionData
    {
        public string CommandType { get; }
        public object Data { get; }

        public InteractionClientActionData(
            string commandType,
            object data
        )
        {
            CommandType = commandType;
            Data = data;
        }
    }
}