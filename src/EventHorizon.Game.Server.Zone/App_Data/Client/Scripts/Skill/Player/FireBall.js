/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: IObjectEntity;
 */

const skillId = "Skills_FireBall.json";
const entity = $data;
const noSelectedTargetMessage = $services.i18n["noTargetSelected"];

$utils.runClientScript(
    "skill.companion_targeted_skill",
    "Skill_Runners_RunSelectedTargetedSkill.js",
    {
        entity,
        skillId,
        noSelectedTargetMessage,
    }
);
