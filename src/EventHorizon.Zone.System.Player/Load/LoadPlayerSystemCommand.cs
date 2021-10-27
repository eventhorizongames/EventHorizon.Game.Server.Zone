namespace EventHorizon.Zone.System.Player.Load
{
    using EventHorizon.Zone.Core.Model.Command;

    using global::System;

    using MediatR;

    public record LoadPlayerSystemCommand
        : IRequest<LoadPlayerSystemResult>;

    public class LoadPlayerSystemResult
        : StandardCommandResult
    {
        public bool WasUpdated { get; }
        public string[] ReasonCode { get; }

        public LoadPlayerSystemResult(
            bool wasUpdated,
            string[] reasonCode
        )
        {
            WasUpdated = wasUpdated;
            ReasonCode = reasonCode;
        }

        public LoadPlayerSystemResult(
            string errorCode
        ) : base(errorCode)
        {
            WasUpdated = false;
            ReasonCode = Array.Empty<string>();
        }

        public static implicit operator LoadPlayerSystemResult(
            string errorCode
        ) => new(
            errorCode
        );
    }
}
