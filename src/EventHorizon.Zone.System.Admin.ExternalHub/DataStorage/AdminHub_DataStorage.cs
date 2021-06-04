namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.DataStorage.Events.Create;
    using EventHorizon.Zone.System.DataStorage.Events.Delete;
    using EventHorizon.Zone.System.DataStorage.Events.Query;
    using EventHorizon.Zone.System.DataStorage.Events.Update;
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    /// <summary>
    /// Make sure this stays on the ExternalHub root namespace.
    /// This Class is encapsulating the Zone Info related logic,
    ///  and allows for a single SignalR hub to host all APIs.
    /// </summary>
    public partial class AdminHub
    {
        public async Task<CommandResult<IReadOnlyDictionary<string, object>>> DataStorage_All()
            => await _mediator.Send(
                new QueryForAllDataStoreValues(),
                Context.ConnectionAborted
            );

        public async Task<StandardCommandResult> DataStorage_Create(
            string key,
            string type,
            object value
        ) => await _mediator.Send(
            new CreateDataStoreValueCommand(
                key,
                type,
                value
            ),
            Context.ConnectionAborted
        );

        public async Task<StandardCommandResult> DataStorage_Update(
            string key,
            string type,
            object value
        ) => await _mediator.Send(
            new UpdateDataStoreValueCommand(
                key,
                type,
                value
            ),
            Context.ConnectionAborted
        );

        public async Task<StandardCommandResult> DataStorage_Delete(
            string key
        ) => await _mediator.Send(
            new DeleteDataStoreValueCommand(
                key
            ),
            Context.ConnectionAborted
        );

    }
}
