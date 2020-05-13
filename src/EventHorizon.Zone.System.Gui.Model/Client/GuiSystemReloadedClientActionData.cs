namespace EventHorizon.Zone.System.Gui.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;
    using global::System.Collections.Generic;

    public class GuiSystemReloadedClientActionData : IClientActionData
    {
        public IEnumerable<GuiLayout> GuiLayoutList { get; }

        public GuiSystemReloadedClientActionData(
            IEnumerable<GuiLayout> guiLayoutList
        )
        {
            GuiLayoutList = guiLayoutList;
        }
    }
}
