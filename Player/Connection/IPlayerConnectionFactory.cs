using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Game.Server.Core.Player.Connection
{
    public interface IPlayerConnectionFactory
    {
        Task<IPlayerConnection> GetConnection();
    }
}