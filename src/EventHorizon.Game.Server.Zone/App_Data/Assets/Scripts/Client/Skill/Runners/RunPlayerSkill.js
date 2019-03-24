/** 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 *  casterId: number;
 *  targetId: number;
 *  skillId: string;
 * };
 */

$services.eventService.publish(
    $utils.createEvent("PLAYER.ACTION_EVENT", {
        action: "Player.RUN_SKILL",
        data: $data
    })
);