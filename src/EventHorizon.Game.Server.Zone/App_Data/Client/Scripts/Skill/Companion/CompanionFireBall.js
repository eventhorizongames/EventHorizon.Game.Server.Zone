/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: IObjectEntity;
 */

const entity = $data;
const skillId = "Skills_FireBall.json";
const noSelectionsMessage = $services.i18n["noSelectionsMessage"];

$utils.runClientScript(
    "skill.companion_targeted_skill",
    "Skill_Runners_RunSelectedCompanionTargetedSkill.js",
    {
        entity,
        skillId,
        noSelectionsMessage,
    }
);
