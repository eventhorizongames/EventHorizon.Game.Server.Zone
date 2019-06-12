using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Admin;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;

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