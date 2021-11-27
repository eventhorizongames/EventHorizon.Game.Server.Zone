namespace EventHorizon.Zone.System.Template.Timer;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Template.Background;

using MediatR;

public class TemplateBackgroundTimer
    : ITimerTask
{
    public int Period { get; } = 5000;
    public string Tag { get; } = nameof(TemplateBackgroundTimer);
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new TemplateBackgroundEvent();
    public bool LogDetails { get; }
}
