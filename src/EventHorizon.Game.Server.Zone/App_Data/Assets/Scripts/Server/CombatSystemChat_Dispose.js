/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 * $state: {
 *  onMessageFromCombatSystemHandler: (data) => void;
 * }
 */

$services.logger.debug("Hello from Server Dispose", $services);

$services.eventService.removeEventListener({
        key: "MessageFromCombatSystem"
    },
    $data.onMessageFromCombatSystemHandler,
    $data.onMessageFromCombatSystemContext
);