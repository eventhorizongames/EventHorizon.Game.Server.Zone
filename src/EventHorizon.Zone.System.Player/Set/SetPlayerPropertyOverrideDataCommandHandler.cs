namespace EventHorizon.Zone.System.Player.Set
{
    using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Api;

    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class SetPlayerPropertyOverrideDataCommandHandler
        : IRequestHandler<SetPlayerPropertyOverrideDataCommand, CommandResult<PlayerEntity>>
    {
        private readonly PlayerSettingsCache _cache;

        public SetPlayerPropertyOverrideDataCommandHandler(
            PlayerSettingsCache cache
        )
        {
            _cache = cache;
        }

        public Task<CommandResult<PlayerEntity>> Handle(
            SetPlayerPropertyOverrideDataCommand request,
            CancellationToken cancellationToken
        )
        {
            var forceSetList = _cache.PlayerData.ForceSet;
            foreach (var property in _cache.PlayerData.Where(
                a => a.Key.StartsWith(
                    nameof(ObjectEntityData.ForceSet).LowercaseFirstChar()
                )
            ))
            {
                if (!request.PlayerEntity.RawData.ContainsKey(
                    property.Key
                ) || forceSetList.Contains(
                    property.Key
                ))
                {
                    request.PlayerEntity.RawData[property.Key] = property.Value;
                }
            }

            return new CommandResult<PlayerEntity>(
                request.PlayerEntity
            ).FromResult();
        }
    }
}
