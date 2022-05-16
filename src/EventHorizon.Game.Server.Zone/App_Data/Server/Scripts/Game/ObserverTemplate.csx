// using System.Threading.Tasks;
// using Collections = System.Collections.Generic;
// using Logging = Microsoft.Extensions.Logging;

// using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;
// using FindEntity = EventHorizon.Zone.Core.Events.Entity.Find;
// using FindPlayer = EventHorizon.Zone.System.Player.Events.Find;
// using EntityModel = EventHorizon.Zone.Core.Model.Entity;

// public class __SCRIPT__
//     : ScriptsModel.ObserverableMessageBase<
//           __SCRIPT__,
//           __SCRIPT__Observer
//       >
// {
//     public __SCRIPT__(
//     )
//     {
//     }
// }

// public interface __SCRIPT__Observer
//     : ObserverModel.ArgumentObserver<__SCRIPT__> { }

// public class __SCRIPT__Handler
//     : ScriptsModel.ServerScript,
//       __SCRIPT__Observer
// {
//     public string Id => "__SCRIPT__";
//     public Collections.IEnumerable<string> Tags =>
//         new Collections.List<string> { };

//     private ScriptsModel.ServerScriptServices _services;
//     private Logging.ILogger _logger;

//     public async Task<ServerScriptResponse> Run(
//         ScriptsModel.ServerScriptServices services,
//         ScriptsModel.ServerScriptData data
//     )
//     {
//         _services = services;
//         _logger = services.Logger<__SCRIPT__>();
//         _logger.LogDebug("__SCRIPT__ - Server Script");

//         return new ScriptsModel.StandardServerScriptResponse(
//             true,
//             "observer_setup"
//         );
//     }

//     public async Task Handle(__SCRIPT__ args)
//     {
//         // START - Insert Code Here
        
//         // END - Insert Code Here
//     }
// }
