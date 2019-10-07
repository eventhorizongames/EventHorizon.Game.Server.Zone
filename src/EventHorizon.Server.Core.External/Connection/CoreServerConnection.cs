using System;
using System.Threading.Tasks;

namespace EventHorizon.Server.Core.External.Connection
{
    public interface CoreServerConnection
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