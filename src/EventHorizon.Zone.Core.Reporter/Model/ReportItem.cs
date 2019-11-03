namespace EventHorizon.Zone.Core.Reporter.Model
{
    public struct ReportItem
    {
        public string Message { get; }
        public string Data { get; }

        public ReportItem(
            string message, 
            string data
        )
        {
            Message = message;
            Data = data;
        }
    }
}