/**
 * Action Id: message_client
 * 
 * Services:
 * - EntityRepository
 * Methods:
 * - sendEvent(eventName: string, data?: any): void;
 * - resolveTemplate(templateAsString: string, data: { [key]: string }): string;
 * Data: 
 * - messageCode: string
 * - messageTemplate: string
 * - templateData: { [key]: string }
 */

// TODO: LOCALIZATION -- use the messageCode to get the messageTemplate
Methods.sendEvent(
    "MessageFromCombatSystem", {
        messageCode: Data.messageCode,
        // This is done twice so this supports message templates inside template data.
        message: Methods.resolveTemplate(
            Methods.resolveTemplate(Data.messageTemplate, Data.templateData),
            Data.templateData
        )
    }
);