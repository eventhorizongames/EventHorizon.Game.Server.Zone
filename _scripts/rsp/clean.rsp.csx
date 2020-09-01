#!dotnet-script
// Do not run this scripts directly, it will be called by "_scripts/run.script.generation.services.ps1".

using System;
using System.Collections.Generic;
using System.IO;

CleanupDirectory(
    "src/EventHorizon.Zone.System.Server.Scripts.Services/bin/Debug/netstandard2.1/publish"
);
CleanupDirectory(
    "src/EventHorizon.Zone.System.Client.Scripts.Services/bin/Debug/netstandard2.1/publish"
);

void CleanupDirectory(
    string directoryToClean
)
{
    Directory.Delete(
        directoryToClean,
        true
    );
    Console.WriteLine(
        "Cleaned Published Directory: \n\t{0}",
        directoryToClean
    );
}