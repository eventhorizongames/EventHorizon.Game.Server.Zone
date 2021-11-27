namespace EventHorizon.Zone.System.Template.Load;

using EventHorizon.Zone.Core.Model.Command;

using global::System;

using MediatR;

public record LoadTemplateSystemCommand
    : IRequest<LoadTemplateSystemResult>;

public class LoadTemplateSystemResult
    : StandardCommandResult
{
    public bool WasUpdated { get; }
    public string[] ReasonCode { get; }

    public LoadTemplateSystemResult(
        bool wasUpdated,
        string[] reasonCode
    )
    {
        WasUpdated = wasUpdated;
        ReasonCode = reasonCode;
    }

    public LoadTemplateSystemResult(
        string errorCode
    ) : base(errorCode)
    {
        WasUpdated = false;
        ReasonCode = Array.Empty<string>();
    }

    public static implicit operator LoadTemplateSystemResult(
        string errorCode
    ) => new(
        errorCode
    );
}
