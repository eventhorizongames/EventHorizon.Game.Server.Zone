using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Core.Model
{
    public class ZoneRegistrationDetails
    {
        public string ServerAddress { get; set; }
        public IList<string> Tags { get; set; }
    }
}