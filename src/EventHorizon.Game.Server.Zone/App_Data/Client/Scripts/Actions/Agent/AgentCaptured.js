/**
 * Action Id: agent_captured
 *
 * $services:
 * - EntityRepository
 * $utils:
 * - sendEvent(eventName: string, data?: any): void;
 * - resolveTemplate(templateAsString: string, data: { [key]: string }): string;
 * $data:
 * - messageCode: string
 * - messageTemplate: string
 * - templateData: { [key]: string }
 */

$utils.sendEvent('CLEAR_POINTER_HIT_ENTITY_EVENT', {});
