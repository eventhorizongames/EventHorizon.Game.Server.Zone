using System;

using Microsoft.Extensions.Logging;

namespace EventHorizon.Server.Core.External.Tests.TestUtil
{
    public class TestingLogger : ILogger
    {
        public IDisposable BeginScope<TState>(
            TState state
        )
        {
            return null;
        }

        public bool IsEnabled(
            LogLevel logLevel
        )
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter
        )
        {
            // Just ignore the Log message, we are only testing.
        }
    }
}
