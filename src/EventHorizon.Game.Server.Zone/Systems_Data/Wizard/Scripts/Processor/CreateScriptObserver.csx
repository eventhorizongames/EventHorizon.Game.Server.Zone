/// <summary>
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Wizard.Model.Scripts;

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "SYSTEM", "wizard-processor" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - System Script");
        // var wizard = data.Get<WizardMetadata>("Wizard");
        // var wizardStep = data.Get<WizardStep>("WizardStep");
        // var wizardData = data.Get<WizardData>("WizardData");

        return new WizardServerScriptResponse(
            true,
            string.Empty
        );
    }
}
