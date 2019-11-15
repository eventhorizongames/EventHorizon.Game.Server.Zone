using MediatR;

namespace EventHorizon.Zone.Core.Events.FileService
{
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