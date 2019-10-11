using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Player.Connection.Internal
{
    public class SystemPlayerServerConnection : PlayerServerConnection
    {
        readonly ILogger _logger;
        readonly HubConnection _connection;

        public SystemPlayerServerConnection(
            ILogger logger,
            HubConnection connection
        )
        {
            _logger = logger;
            _connection = connection;
        }

        public void OnAction<T>(
            string actionName,
            Action<T> action
        )
        {
            try
            {
                _connection.On<T>(
                    actionName,
                    action
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Generic On Action failed",
                    ex
                );
                throw ex;
            }
        }
        public void OnAction(
            string actionName,
            Action action
        )
        {
            try
            {
                _connection.On(
                    actionName,
                    action
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "On Action failed",
                    ex
                );
                throw ex;
            }
        }

        public async Task<T> SendAction<T>(
            string actionName,
            object[] args
        )
        {
            try
            {
                return await _connection.InvokeCoreAsync<T>(
                    actionName,
                    args
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Generic Send Action failed",
                    ex
                );
                throw ex;
            }
        }

        public async Task SendAction(
            string actionName,
            object[] args
        )
        {
            try
            {
                await _connection.InvokeCoreAsync(
                    actionName,
                    args
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Send Action failed",
                    ex
                );
                throw ex;
            }
        }
    }
}