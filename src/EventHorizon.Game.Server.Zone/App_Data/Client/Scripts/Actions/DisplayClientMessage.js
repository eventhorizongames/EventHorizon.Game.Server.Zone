/**
 * Action Id: message_client
 *
 * $services:
 * - EntityRepository
 * $utils:
 * - sendEvent(eventName: string, data?: any): void;
 * - resolveTemplate(templateAsString: string, data: { [key]: string }): string;
 * - translate(key: string, replaceArray: Array<string | number>): string;
 * $data:
 * - MessageKey: string
 * - MessageData: { [key]: string }
 */

$utils.sendEvent("DisplayFeedbackMessage", {
    message: $utils.resolveTemplate(
        $utils.translation($data.messageKey),
        $data.messageData
    ),
});
