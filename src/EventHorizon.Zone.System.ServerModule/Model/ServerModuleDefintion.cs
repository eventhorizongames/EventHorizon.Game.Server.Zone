using System;
using System.IO;

namespace EventHorizon.Zone.System.ServerModule.Model
{
    public struct ServerModuleDefintion
    {
        public string Name { get; set; }
        public string InitializeScript { get; set; }
        public string DisposeScript { get; set; }
        public string UpdateScript { get; set; }

        public ServerModuleScripts CreateScripts(string scriptsPath)
        {
            return new ServerModuleScripts
            {
                Name = this.Name,
                InitializeScriptString = GetScriptFileContent(scriptsPath, this.InitializeScript),
                DisposeScriptString = GetScriptFileContent(scriptsPath, this.DisposeScript),
                UpdateScriptString = GetScriptFileContent(scriptsPath, this.UpdateScript),
            };
        }

        private string GetScriptFileContent(string scriptsPath, string scriptFileName)
        {
            try
            {
                var fileName = Path.Combine(
                    scriptsPath, scriptFileName
                );
                using (var file = File.OpenText(fileName))
                {
                    return file.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Exception with {Name}",
                    ex
                );
            }
        }
    }
}