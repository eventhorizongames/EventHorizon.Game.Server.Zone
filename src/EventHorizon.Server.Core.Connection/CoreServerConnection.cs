namespace EventHorizon.Server.Core.Connection;

using System;
using System.Threading.Tasks;

public interface CoreServerConnection
{
    CoreServerConnectionApi Api { get; }

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
