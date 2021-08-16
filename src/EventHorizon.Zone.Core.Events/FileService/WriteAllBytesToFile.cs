namespace EventHorizon.Zone.Core.Events.FileService
{
    using System;

    using MediatR;

    public struct WriteAllBytesToFile
        : IRequest
    {
        public string FileFullName { get; }
        public byte[] Bytes { get; }

        public WriteAllBytesToFile(
            string fileFullName,
            byte[] fileContent
        )
        {
            FileFullName = fileFullName;
            Bytes = fileContent;
        }
    }
}
