namespace EventHorizon.Zone.System.Wizard.Json.Mapper
{
    using EventHorizon.Zone.System.Wizard.Json.Model;
    using EventHorizon.Zone.System.Wizard.Model;
    using MediatR;

    public struct MapWizardDataToJsonDataDocument
        : IRequest<JsonDataDocument>
    {
        public WizardData WizardData { get; }

        public MapWizardDataToJsonDataDocument(
            WizardData wizardData
        )
        {
            WizardData = wizardData;
        }
    }
}
