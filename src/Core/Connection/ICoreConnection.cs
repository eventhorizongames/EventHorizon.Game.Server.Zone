using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Game.Server.Zone.Core.Connection
{
    public interface ICoreConnection
    {
        void OnAction<T>(string actionName, Action<T> action);
        void OnAction(string actionName, Action action);
        Task<T> SendAction<T>(string actionName, params object[] args);
        Task SendAction(string actionName, params object[] args);
    }
}