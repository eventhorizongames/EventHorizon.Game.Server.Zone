namespace EventHorizon.Zone.Core.Entity.Load
{
    using System;

    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public record LoadEntityCoreCommand
        : IRequest<LoadEntityCoreResult>;

    public class LoadEntityCoreResult
        : StandardCommandResult
    {
        public bool WasUpdated { get; }
        public string[] ReasonCode { get; }

        public LoadEntityCoreResult(
            bool wasUpdated,
            string[] reasonCode
        )
        {
            WasUpdated = wasUpdated;
            ReasonCode = reasonCode;
        }

        public LoadEntityCoreResult(
            string errorCode
        ) : base(errorCode)
        {
            WasUpdated = false;
            ReasonCode = Array.Empty<string>();
        }

        public static implicit operator LoadEntityCoreResult(
            string errorCode
        ) => new(
            errorCode
        );
    }
}
