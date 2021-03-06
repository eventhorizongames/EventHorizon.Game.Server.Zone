namespace EventHorizon.Zone.Core.Plugin.LocalFileSystem;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

public class LocalFileSystemDirectoryResolver
    : DirectoryResolver
{
    public bool CreateDirectory(
        string directoryFullName
    ) => Directory.CreateDirectory(
        directoryFullName
    ).Exists;

    public void DeleteDirectory(
        string directoryFullName,
        bool recursive = false
    ) => Directory.Delete(
        directoryFullName,
        recursive
    );

    public bool DoesDirectoryExist(
        string directoryFullName
    ) => Directory.Exists(
        directoryFullName
    );

    public IEnumerable<StandardDirectoryInfo> GetDirectories(
        string directoryFullName
    ) => new DirectoryInfo(
        directoryFullName
    ).GetDirectories().Select(
        directory => new StandardDirectoryInfo(
            directory.Name,
            directory.FullName,
            directory.Parent?.FullName ?? string.Empty
        )
    );

    public StandardDirectoryInfo GetDirectoryInfo(
        string directoryFullName
    )
    {
        var directoryInfo = new DirectoryInfo(
            directoryFullName
        );
        return new StandardDirectoryInfo(
            directoryInfo.Name,
            directoryInfo.FullName,
            directoryInfo.Parent?.FullName ?? string.Empty
        );
    }

    public IEnumerable<StandardFileInfo> GetFiles(
        string directoryFullName
    ) => new DirectoryInfo(
        directoryFullName
    ).GetFiles().Select(
        file => new StandardFileInfo(
            file.Name,
            file.DirectoryName ?? string.Empty,
            file.FullName,
            file.Extension
        )
    );

    public bool IsEmpty(
        string directoryFullName
    )
    {
        var directoryInfo = new DirectoryInfo(
            directoryFullName
        );
        return !directoryInfo.Exists
            || directoryInfo.GetFiles().Length <= 0
            && directoryInfo.GetDirectories().Length <= 0;
    }
}
