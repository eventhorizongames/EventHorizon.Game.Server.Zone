namespace EventHorizon.Zone.System.Template.Background;

using EventHorizon.Observer.Model;

using MediatR;

public struct TemplateBackgroundEvent
        : INotification
{

}

public interface TemplateBackgroundEventObserver
    : ArgumentObserver<TemplateBackgroundEvent>
{
}
