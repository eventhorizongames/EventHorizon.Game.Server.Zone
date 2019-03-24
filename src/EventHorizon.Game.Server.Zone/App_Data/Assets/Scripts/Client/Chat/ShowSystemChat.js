/** 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 * };
 */

$services.eventService.publish(
    $utils.createEvent(
        "MessageFromCombatSystem.SHOW"
    )
);