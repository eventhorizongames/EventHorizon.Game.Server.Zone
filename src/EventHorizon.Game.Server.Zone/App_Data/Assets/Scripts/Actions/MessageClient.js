// Action Id: message_client
// Services:
// - EntityRepository
// Methods:
// - sendEvent(eventName: string, data?: any): void;
// - resolveTemplate(templateAsString: string, data: { [key]: string }): string;
// Data: 
// - messageCode: string
// - messageTemplate: string
// - templateData: { [key]: string }

// TODO: LOCALIZATION -- use the messageCode to get the messageTemplate
Methods.sendEvent(
    "MessageToCombatSystemLog", {
        messageCode: Data.messageCode,
        message: Methods.resolveTemplate(Data.messageTemplate, Data.templateData)
    }
);