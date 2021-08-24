namespace EventHorizon.Monitoring.ApplicationInsights.Tests.Utils
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;

    public class TelemetryChannelMock : ITelemetryChannel
    {
        public IList<ITelemetry> Items
        {
            get;
            private set;
        } = new List<ITelemetry>();

        public void Send(ITelemetry item)
        {
            Items.Add(item);
        }

        public bool? DeveloperMode { get; set; }
        public string EndpointAddress { get; set; }


        public void Flush()
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool _disposedValue; 

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
