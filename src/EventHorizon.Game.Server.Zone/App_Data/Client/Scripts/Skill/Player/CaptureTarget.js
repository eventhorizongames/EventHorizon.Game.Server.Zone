/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: IObjectEntity;
 */

const skillId = "Skills_CaptureTarget.json";
const entity = $data;
const noSelectedTargetMessage = $services.i18n["noTargetToCaptureSelected"];

$utils.runClientScript(
    "skill.targeted_skill",
    "Skill_Runners_RunSelectedTargetedSkill.js",
    {
        entity,
        skillId,
        noSelectedTargetMessage,
    }
);
