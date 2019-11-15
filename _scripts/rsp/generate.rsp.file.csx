#!dotnet-script
// Do not run this scripts directly, it will be called by "_scripts/run.script.generation.services.ps1".
// Running th generation script will setup the RSP file, this file is used by the CSX scripting services
//  to help with project intellisense.

using System;
using System.Collections.Generic;
using System.IO;

var fileContentLines = new Dictionary<string, string>()
{
    {   "load.System", "/r:System" },
    {   "using.System", "/u:System" }
};

// Generate EventHorizon Script Services DLL dependencies
var allServiesFiles = Directory.GetFiles(
    "src/EventHorizon.Zone.System.Server.Scripts.Services/bin/Debug/netstandard2.1/publish",
    "*.dll",
    SearchOption.TopDirectoryOnly
);

foreach (var fileName in allServiesFiles)
{
    var file = new FileInfo(
        fileName
    );
    var stringToWrite = $"/r:src/EventHorizon.Zone.System.Server.Scripts.Services/bin/Debug/netstandard2.1/publish/{file.Name}";
    if (!fileContentLines.ContainsKey($"load.{file.Name}"))
    {
        fileContentLines.Add(
            $"load.{file.Name}",
            stringToWrite
        );
    }
}

// Generate EventHorizon Zone Core DLL dependencies
var allCoreFiles = Directory.GetFiles(
    "src/EventHorizon.Game.Server.Zone/bin/Debug/netcoreapp3.0/",
    "EventHorizon.*.dll",
    SearchOption.TopDirectoryOnly
);

foreach (var fileName in allCoreFiles)
{
    var file = new FileInfo(
        fileName
    );
    var stringToWrite = $"/r:src/EventHorizon.Game.Server.Zone/bin/Debug/netcoreapp3.0/{file.Name}";
    if (!fileContentLines.ContainsKey($"load.{file.Name}"))
    {
        fileContentLines.Add(
            $"load.{file.Name}",
            stringToWrite
        );
    }
}


// Map the lines into a single content file
var fileContent = "";
foreach (var fileContentLine in fileContentLines)
{
    fileContent += $"{fileContentLine.Value}\n";
    
}

var fileToCreate = "script.rsp";
if (File.Exists(fileToCreate))
{
    File.Delete(
        fileToCreate
    );
}
File.WriteAllText(
    fileToCreate,
    fileContent
);

Console.WriteLine(
    "Content Written Out: \n{0}",
    fileContent
);