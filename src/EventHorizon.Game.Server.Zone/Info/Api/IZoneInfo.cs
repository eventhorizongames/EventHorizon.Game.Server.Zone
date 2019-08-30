using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Map.State;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Particle;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.ClientEntities.Api;
using EventHorizon.Zone.System.ClientEntity.Api;
using EventHorizon.Zone.System.EntityModule.Model;
using EventHorizon.Zone.System.Gui.Model;
using EventHorizon.Zone.System.ServerModule.Model;

namespace EventHorizon.Game.Server.Zone.Info.Api
{
    public interface IZoneInfo
    {
        PlayerEntity Player { get; }
        IDictionary<string, string> I18nMap { get; }
        ZoneMapMesh MapMesh { get; }
        MapGraph Map { get; }
        List<IObjectEntity> EntityList { get; }
        IEnumerable<GuiLayout> GuiLayoutList { get; }
        IEnumerable<ParticleTemplate> ParticleTemplateList { get; }
        IEnumerable<ServerModuleScripts> ServerModuleScriptList { get; }
        IEnumerable<ClientScript> ClientScriptList { get; }
        IEnumerable<IClientAsset> ClientAssetList { get; }
        IEnumerable<IClientEntityInstance> ClientEntityInstanceList { get; }
        IEnumerable<EntityScriptModule> BaseEntityScriptModuleList { get; }
        IEnumerable<EntityScriptModule> PlayerEntityScriptModuleList { get; }
    }
}