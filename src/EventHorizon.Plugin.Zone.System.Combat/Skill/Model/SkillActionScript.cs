using System;
using System.IO;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SkillActionScript
    {
        public string Id { get; set; }
        public string ScriptFile { get; set; }
        public string ScriptString { get; set; }

        public void CreateScript(string scriptPath)
        {
            try
            {
                this.ScriptString = File.OpenText(
                    Path.Combine(
                        scriptPath,
                        ScriptFile
                    )
                ).ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {Id}",
                    ex
                );
            }
        }
    }
}