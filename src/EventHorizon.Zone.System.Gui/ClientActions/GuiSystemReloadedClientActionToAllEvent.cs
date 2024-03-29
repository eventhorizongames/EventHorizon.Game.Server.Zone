﻿namespace EventHorizon.Zone.System.Gui.ClientActions;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.Gui.Model.Client;

public static class GuiSystemReloadedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        GuiSystemReloadedClientActionData data
    ) => new(
        "GUI_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
        data
    );
}
