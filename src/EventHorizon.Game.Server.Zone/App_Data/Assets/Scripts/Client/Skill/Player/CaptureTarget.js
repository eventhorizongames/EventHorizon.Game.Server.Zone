/** 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: IObjectEntity;
 */

const skillId = "capture_target";
const entity = $data;
const noSelectedTargetMessage = $services.i18n["noTargetToCaptureSelected"];

$utils.runClientScript(
    "skill.targeted_skill",
    "Client_Skill_Runners_RunSelectedTargetedSkill.js", {
        entity,
        skillId,
        noSelectedTargetMessage
    }
);