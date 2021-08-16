using MediatR;

namespace EventHorizon.Zone.Core.Events.FileService
{
    public struct AppendTextToFile : IRequest<bool>
    {
        public string FileFullName { get; }
        public string Text { get; }

        public AppendTextToFile(
            string fileFullName,
            string text
        )
        {
            FileFullName = fileFullName;
            Text = text;
        }
    }
}
