using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Info.Api;
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

namespace EventHorizon.Game.Server.Zone.Info.Model
{
    public struct ZoneInfo : IZoneInfo
    {
        public PlayerEntity Player { get; set; }
        public IDictionary<string, string> I18nMap { get; set; }
        public ZoneMapMesh MapMesh { get; set; }
        public MapGraph Map { get; set; }
        public List<IObjectEntity> EntityList { get; set; }
        public IEnumerable<GuiLayout> GuiLayoutList { get; set; }
        public IEnumerable<ParticleTemplate> ParticleTemplateList { get; set; }
        public IEnumerable<ServerModuleScripts> ServerModuleScriptList { get; set; }
        public IEnumerable<ClientScript> ClientScriptList { get; set; }
        public IEnumerable<IClientAsset> ClientAssetList { get; set; }
        public IEnumerable<IClientEntityInstance> ClientEntityInstanceList { get; set; }
        public IEnumerable<EntityScriptModule> BaseEntityScriptModuleList { get; set; }
        public IEnumerable<EntityScriptModule> PlayerEntityScriptModuleList { get; set; }
    }
}