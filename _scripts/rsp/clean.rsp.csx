#!dotnet-script
// Do not run this scripts directly, it will be called by "_scripts/run.script.generation.services.ps1".

using System;
using System.Collections.Generic;
using System.IO;

var publishedFolder = "src/EventHorizon.Zone.System.Server.Scripts.Services/bin/Debug/netstandard2.0/publish";
// Remove the Publish folder for Script Services
Directory.Delete(
    publishedFolder,
    true
);

Console.WriteLine(
    "Cleaned Published Folder: \n{0}",
    publishedFolder
);