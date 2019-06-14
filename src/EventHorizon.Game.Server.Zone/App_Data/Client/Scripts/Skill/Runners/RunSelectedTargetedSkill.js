/** 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 *  entity: IObjectEntity;
 *  skillId: string;
 *  noSelectedTargetMessage: string;
 * };
 */

const skillId = $data.skillId;
const entity = $data.entity;
const message = $data.noSelectedTargetMessage;
const selectedTrackerModule = entity.getProperty(
    "SELECTED_TRACKER_MODULE_NAME"
);
if (selectedTrackerModule.hasSelectedEntity) {
    $utils.runClientScript(
        "skill.targeted_skill",
        "Skill_Runners_RunPlayerSkill.js", {
            casterId: entity.entityId,
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