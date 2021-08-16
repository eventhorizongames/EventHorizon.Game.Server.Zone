using System;
using System.Threading.Tasks;

namespace EventHorizon.Zone.System.Player.Connection
{
    public interface PlayerServerConnection
    {
        void OnAction<T>(
            string actionName,
            Action<T> action
        );
        void OnAction(
            string actionName,
            Action action
        );
        Task<T> SendAction<T>(
            string actionName,
            params object[] args
        );
        Task SendAction(
            string actionName,
            params object[] args
        );
    }
}
