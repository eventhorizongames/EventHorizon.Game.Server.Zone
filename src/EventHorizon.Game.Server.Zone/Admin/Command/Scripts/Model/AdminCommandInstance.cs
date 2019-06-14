using System;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model
{
    public struct AdminCommandInstance
    {
        public string Command { get; set; }
        public string ScriptFile { get; set; }
        public bool IsFound()
        {
            return this.Command != null;
        }
    }
}