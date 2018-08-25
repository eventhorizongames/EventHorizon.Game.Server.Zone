using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Player.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Mapper
{
    public class AgentFromEntityToDetails
    {
        public static AgentDetails Map(AgentEntity entity)
        {
            // TODO: Add validation to Entity.
            return new AgentDetails
            {
                Name = entity.Name,
                Position = entity.Position,
                Data = entity.Data,
                Speed = entity.Speed,
                Ai = entity.Ai,
            };
        }
    }
}