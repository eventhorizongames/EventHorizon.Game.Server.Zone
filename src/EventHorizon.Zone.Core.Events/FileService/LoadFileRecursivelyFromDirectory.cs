using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

namespace EventHorizon.Zone.Core.Events.FileService
{
    public struct LoadFileRecursivelyFromDirectory : IRequest
    {
        public string FromDirectory { get; }
        public Func<StandardFileInfo, IDictionary<string, object>, Task> OnProcessFile { get; }
        public IDictionary<string, object> Arguments { get; }

        public LoadFileRecursivelyFromDirectory(
            string fromDirectory,
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile,
            IDictionary<string, object> arguments
        )
        {
            FromDirectory = fromDirectory;
            OnProcessFile = onProcessFile;
            Arguments = arguments;
        }
    }
}