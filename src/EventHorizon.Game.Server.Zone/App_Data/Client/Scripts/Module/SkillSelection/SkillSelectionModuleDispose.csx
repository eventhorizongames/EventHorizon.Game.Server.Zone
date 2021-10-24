/*
 * id: SkillSelection_SkillSelectionModuleDispose
data:
    active: bool
    observer: ObserverBase
    skillList: IList<SkillDetails>
*/

using System.Threading.Tasks;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("SkillSelection Module - Dispose Script");

        var layoutId = "GUI_Module_SkillSelection.json";
        var guiId = layoutId;

        await services.Mediator.Send(
            new DisposeOfGuiCommand(
                guiId
            )
        );

        UnRegisterObserver(
            services,
            data,
            "observer"
        );
    }

    private void UnRegisterObserver(
        ScriptServices services,
        ScriptData data,
        string observerName
    )
    {
        var observer = data.Get<ObserverBase>(
            observerName
        );

        if (observer != null)
        {
            services.UnRegisterObserver(
                observer
            );
        }
    }
}