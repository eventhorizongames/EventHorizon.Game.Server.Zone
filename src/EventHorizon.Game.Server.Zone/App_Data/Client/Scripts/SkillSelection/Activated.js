/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: {
 *  layout: IGuiLayoutData
 * };
 */
$services.logger.debug("Activated.js", { $data });

$services.eventService.publish(
    $utils.createEvent("Local.SkillSelection.Gui.ACTIVATED", {
        id: $data,
    })
);
