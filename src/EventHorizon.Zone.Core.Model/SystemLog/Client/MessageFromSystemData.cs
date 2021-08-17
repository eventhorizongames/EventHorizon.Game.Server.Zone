namespace EventHorizon.Zone.Core.Model.SystemLog.Client
{
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.Client;

    public struct MessageFromSystemData : IClientActionData
    {
        public string Message { get; }
        public IDictionary<string, object>? SenderControlOptions { get; }
        public IDictionary<string, object>? MessageControlOptions { get; }

        public MessageFromSystemData(
            string message
        ) : this(message, null, null) { }

        public MessageFromSystemData(
            string message,
            IDictionary<string, object>? senderControlOptions,
            IDictionary<string, object>? messageControlOptions
        )
        {
            Message = message;
            SenderControlOptions = senderControlOptions;
            MessageControlOptions = messageControlOptions;
        }
    }
}
