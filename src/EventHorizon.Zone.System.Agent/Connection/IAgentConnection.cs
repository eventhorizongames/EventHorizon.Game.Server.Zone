namespace EventHorizon.Zone.System.Agent.Connection;

using global::System;
using global::System.Threading.Tasks;

public interface IAgentConnection
{
    void OnAction<T>(string actionName, Action<T> action);
    void OnAction(string actionName, Action action);
    Task<T> SendAction<T>(string actionName, params object[] args);
    Task SendAction(string actionName, params object[] args);
}
