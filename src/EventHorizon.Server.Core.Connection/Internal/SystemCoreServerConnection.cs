namespace EventHorizon.Server.Core.Connection.Internal
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Logging;

    public class SystemCoreServerConnection : CoreServerConnection
    {
        private readonly ILogger _logger;
        private readonly HubConnection _connection;

        public CoreServerConnectionApi Api { get; }

        public SystemCoreServerConnection(
            ILogger logger,
            HubConnection connection
        )
        {
            _logger = logger;
            _connection = connection;
            Api = new SystemCoreServerConnectionApi(
                this
            );
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