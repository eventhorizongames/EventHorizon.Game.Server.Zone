using System;
using System.IO;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Model
{
    public struct SkillInstance
    {
        public static string GenerateId(
            string path,
            string fileName
        )
        {
            var id = string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        new char[] { Path.DirectorySeparatorChar },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                ),
                fileName
            );
            if (id.StartsWith(
                "_"
            ))
            {
                return id.Substring(
                    1
                );
            }
            return id;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SkillValidator[] ValidatorList { get; set; }
        public SkillEffect[] Next { get; set; }
        public SkillEffect[] FailedList { get; set; }

        public bool IsFound()
        {
            return !this.Equals(
                default(SkillInstance)
            );
        }
    }
}