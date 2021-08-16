using System;

namespace EventHorizon.Zone.Core.Reporter.Model
{
    public struct ReportItem
    {
        public string CorrelationId { get; }
        public string Message { get; }
        public DateTime Timestamp { get; set; }
        public object Data { get; }

        public ReportItem(
            string correlationId,
            string message,
            DateTime timestamp,
            object data
        )
        {
            CorrelationId = correlationId;
            Message = message;
            Timestamp = timestamp;
            Data = data;
        }
    }
}
