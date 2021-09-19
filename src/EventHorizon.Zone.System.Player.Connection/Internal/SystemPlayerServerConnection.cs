namespace EventHorizon.Zone.System.Player.Connection.Internal
{
    using global::System;
    using global::System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Logging;

    public class SystemPlayerServerConnection
        : PlayerServerConnection
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
                _connection.On(
                    actionName,
                    action
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Generic On Action failed"
                );
                throw;
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
                    ex,
                    "On Action failed"
                );
                throw;
            }
        }

        public async Task<T?> SendAction<T>(
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
                    ex,
                    "Generic Send Action failed"
                );
                throw;
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
                    ex,
                    "Send Action failed"
                );
                throw;
            }
        }
    }
}
