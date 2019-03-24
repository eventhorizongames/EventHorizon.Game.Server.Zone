/**
 * "Services" Provided by Script system to help with external to script access.
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * };
 * 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 * };
 */

this._keyboardShortcuts.forEach(keyboardShortcut =>
    this._unregisterInput.unregister(keyboardShortcut)
);