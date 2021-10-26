namespace EventHorizon.Zone.System.Player.Load
{
    using EventHorizon.Zone.Core.Model.Command;

    using global::System;

    using MediatR;

    public record LoadSystemPlayerCommand
        : IRequest<LoadSystemPlayerResult>;

    public class LoadSystemPlayerResult
        : StandardCommandResult
    {
        public bool WasUpdated { get; }
        public string[] ReasonCode { get; }

        public LoadSystemPlayerResult(
            bool wasUpdated,
            string[] reasonCode
        )
        {
            WasUpdated = wasUpdated;
            ReasonCode = reasonCode;
        }

        public LoadSystemPlayerResult(
            string errorCode
        ) : base(errorCode)
        {
            WasUpdated = false;
            ReasonCode = Array.Empty<string>();
        }

        public static implicit operator LoadSystemPlayerResult(
            string errorCode
        ) => new(
            errorCode
        );
    }
}
