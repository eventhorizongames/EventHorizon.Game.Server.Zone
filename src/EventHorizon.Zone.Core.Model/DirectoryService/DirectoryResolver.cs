using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.FileService;

namespace EventHorizon.Zone.Core.Model.DirectoryService
{
    public interface DirectoryResolver
    {
        bool DoesDirectoryExist(
            string directoryFullName
        );
        void DeleteDirectory(
            string directoryFullName
        );
        bool CreateDirectory(
            string directoryFullName
        );
        IEnumerable<StandardDirectoryInfo> GetDirectories(
            string directoryFullName
        );
        IEnumerable<StandardFileInfo> GetFiles(
            string directoryFullname
        );
        StandardDirectoryInfo GetDirectoryInfo(
            string directoryFullName
        );
        bool IsEmpty(
            string directoryFullName
        );
    }
}
