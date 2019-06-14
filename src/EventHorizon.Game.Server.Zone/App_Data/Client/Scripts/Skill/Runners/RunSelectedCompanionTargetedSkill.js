/** 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 *  entity: IObjectEntity;
 *  skillId: string;
 *  noSelectionsMessage: string;
 * };
 */

const entity = $data.entity;
const skillId = $data.skillId;
const message = $data.noSelectionsMessage;

const selectedCompanionTrackerModule = entity.getProperty(
    "SELECTED_COMPANION_TRACKER_MODULE_NAME"
);
const selectedTrackerModule = entity.getProperty(
    "SELECTED_TRACKER_MODULE_NAME"
);
if (
    selectedTrackerModule.hasSelectedEntity &&
    selectedCompanionTrackerModule.hasSelectedEntity
) {
    $utils.runClientScript(
        "skill.targeted_skill",
        "Skill_Runners_RunCompanionSkill.js", {
            casterId: selectedCompanionTrackerModule.selectedEntityId,
            targetId: selectedTrackerModule.selectedEntityId,
            skillId
        }
    );
} else {
    $services.eventService.publish(
        $utils.createEvent("MessageFromCombatSystem", {
            message
        })
    );
}