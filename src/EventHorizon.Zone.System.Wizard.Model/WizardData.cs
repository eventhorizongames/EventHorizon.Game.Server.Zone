namespace EventHorizon.Zone.System.Wizard.Model
{
    using global::System.Collections.Generic;

    public class WizardData
        : Dictionary<string, string>
    {
        public WizardData()
            : base()
        { }

        public WizardData(
            IDictionary<string, string> dictionary
        ) : base(
            dictionary
        )
        {
        }
    }
}
