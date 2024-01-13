namespace EventHorizon.Zone.Core.Events.FileService;

using System;
using System.Reflection;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public class WriteResourceToFile
    : IRequest<StandardCommandResult>
{
    public Assembly ExecutingAssembly { get; }
    public string ResourceRoot { get; }
    public string ResourcePath { get; }
    public string ResourceFile { get; }
    public string SaveFileFullName { get; }

    public WriteResourceToFile(
        Assembly executingAssembly,
        string resourceRoot,
        string resourcePath,
        string resourceFile,
        string saveFileFullName
    )
    {
        ExecutingAssembly = executingAssembly;
        ResourceRoot = resourceRoot;
        ResourcePath = resourcePath;
        ResourceFile = resourceFile;
        SaveFileFullName = saveFileFullName;
    }
}
