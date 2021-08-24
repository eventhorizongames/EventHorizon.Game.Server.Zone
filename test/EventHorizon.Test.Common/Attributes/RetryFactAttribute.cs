namespace EventHorizon.Test.Common.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    #region RetryFactAttribute Pulled from https://github.com/xunit/samples.xunit/tree/main/RetryFactExample
    /// <summary>
    /// Works just like [Fact] except that failures are retried (by default, 3 times).
    /// </summary>
    [XunitTestCaseDiscoverer(
        "EventHorizon.Test.Common.Attributes.RetryFactDiscoverer",
        "EventHorizon.Test.Common"
    )]
    public class RetryFactAttribute
        : FactAttribute
    {
        /// <summary>
        /// Number of retries allowed for a failed test. If unset (or set less than 1), will
        /// default to 3 attempts.
        /// </summary>
        public int MaxRetries { get; set; }
    }

    public class RetryFactDiscoverer
        : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public RetryFactDiscoverer(
            IMessageSink diagnosticMessageSink
        )
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo factAttribute
        )
        {
            var maxRetries = factAttribute.GetNamedArgument<int>(
                "MaxRetries"
            );
            if (maxRetries < 1)
                maxRetries = 3;

            yield return new RetryTestCase(
                _diagnosticMessageSink,
                discoveryOptions.MethodDisplayOrDefault(),
                discoveryOptions.MethodDisplayOptionsOrDefault(),
                testMethod,
                maxRetries
            );
        }
    }

    [Serializable]
    public class RetryTestCase
        : XunitTestCase
    {
        private int _maxRetries;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public RetryTestCase() { }

        public RetryTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay testMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod,
            int maxRetries
        ) : base(
            diagnosticMessageSink,
            testMethodDisplay,
            defaultMethodDisplayOptions,
            testMethod,
            testMethodArguments: null
        )
        {
            _maxRetries = maxRetries;
        }

        // This method is called by the xUnit test framework classes to run the test case. We will do the
        // loop here, forwarding on to the implementation in XunitTestCase to do the heavy lifting. We will
        // continue to re-run the test until the aggregator has an error (meaning that some internal error
        // condition happened), or the test runs without failure, or we've hit the maximum number of tries.
        public override async Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource
        )
        {
            var runCount = 0;

            while (true)
            {
                // This is really the only tricky bit: we need to capture and delay messages (since those will
                // contain run status) until we know we've decided to accept the final result;
                var delayedMessageBus = new DelayedMessageBus(
                    messageBus
                );

                var summary = await base.RunAsync(
                    diagnosticMessageSink,
                    delayedMessageBus,
                    constructorArguments,
                    aggregator,
                    cancellationTokenSource
                );
                if (aggregator.HasExceptions
                    || summary.Failed == 0
                    || ++runCount >= _maxRetries
                )
                {
                    delayedMessageBus.Dispose();  // Sends all the delayed messages
                    return summary;
                }

                diagnosticMessageSink.OnMessage(
                    new DiagnosticMessage(
                        "Execution of '{0}' failed (attempt #{1}), retrying...",
                        DisplayName,
                        runCount
                    )
                );
            }
        }

        public override void Serialize(
            IXunitSerializationInfo data
        )
        {
            base.Serialize(
                data
            );

            data.AddValue(
                "MaxRetries",
                _maxRetries
            );
        }

        public override void Deserialize(
            IXunitSerializationInfo data
        )
        {
            base.Deserialize(
                data
            );

            _maxRetries = data.GetValue<int>(
                "MaxRetries"
            );
        }
    }

    /// <summary>
    /// Used to capture messages to potentially be forwarded later. Messages are forwarded by
    /// disposing of the message bus.
    /// </summary>
    public class DelayedMessageBus
        : IMessageBus
    {
        private readonly IMessageBus _innerBus;
        private readonly List<IMessageSinkMessage> _messages = new();

        public DelayedMessageBus(
            IMessageBus innerBus
        )
        {
            _innerBus = innerBus;
        }

        public bool QueueMessage(
            IMessageSinkMessage message
        )
        {
            lock (_messages)
            {
                _messages.Add(
                    message
                );
            }

            // No way to ask the inner bus if they want to cancel without sending them the message, so
            // we just go ahead and continue always.
            return true;
        }

        public void Dispose()
        {
            foreach (var message in _messages)
            {
                _innerBus.QueueMessage(
                    message
                );
            }
        }
    }
    #endregion
}
