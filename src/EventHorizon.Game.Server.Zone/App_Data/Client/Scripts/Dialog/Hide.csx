/*
$services.commandService.send(
    $utils.createEvent("Engine.Gui.HIDE_GUI_COMMAND", { id: "gui_dialog" })
);
*/

using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var guiId = "gui_dialog";
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("Hide: {GuiId}", guiId);

        await services.Mediator.Send(
            new HideGuiCommand(
                guiId
            )
        );
    }
}
