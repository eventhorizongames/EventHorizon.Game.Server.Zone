namespace EventHorizon.Zone.System.Player.Plugin.Action.Model
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

    /// <summary>
    /// This the expected implementation details used to publish Player Action Events.
    /// WARNING: Make sure the implementation is a struct. A struct makes the event immutable using the Set methods.
    /// </summary>
    public interface PlayerActionEvent : INotification
    {
        PlayerEntity Player { get; }
        IDictionary<string, object> Data { get; }

        PlayerActionEvent SetPlayer(
            PlayerEntity player
        );
        PlayerActionEvent SetData(
            IDictionary<string, object> data
        );
    }
}
