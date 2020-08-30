namespace EventHorizon.Zone.Core.Events.FileService
{
    using MediatR;

    public struct WriteAllTextToFile : IRequest
    {
        public string FileFullName { get; }
        public string Text { get; }

        public WriteAllTextToFile(
            string fileFullName,
            string fileContent
        )
        {
            FileFullName = fileFullName;
            Text = fileContent;
        }
    }
}