/**
 * Action Id: message_client
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

// TODO: LOCALIZATION -- use the messageCode to get the messageTemplate
$utils.sendEvent("MessageFromCombatSystem", {
    messageCode: $data.messageCode,
    // This is done twice so this supports message templates inside template data.
    message: $utils.resolveTemplate(
        $utils.resolveTemplate($data.messageTemplate, $data.templateData),
        $data.templateData
    ),
});
