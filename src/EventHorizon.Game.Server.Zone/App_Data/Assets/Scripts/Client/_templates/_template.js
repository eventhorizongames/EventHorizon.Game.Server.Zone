/**
 * "Services" Provided by Script system to help with external to script access.
 * $services: {
 *   i18n: I18nService;
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * };
 * 
 * $utils: {
 *  isObjectDefined(obj: any): bool;
 *  createEvent(event: string, data?: any): IEvent;
 * }
 * 
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 *  Dynamic Based on Script
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 *  Dynamic Based on Script
 * };
 */