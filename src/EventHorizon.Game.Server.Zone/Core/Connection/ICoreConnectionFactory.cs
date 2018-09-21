using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Game.Server.Zone.Core.Connection
{
    public interface ICoreConnectionFactory
    {
        Task<ICoreConnection> GetConnection();
    }
}