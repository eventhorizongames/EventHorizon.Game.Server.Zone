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

        public bool TryGetData(
            string key,
            out string value
        )
        {
            if (TryGetValue(
                key,
                out value
            ))
            {
                return true;
            }
            else if (TryGetValue(
                key.LowercaseFirstChar(),
                out value
            ))
            {
                return true;
            }

            return false;
        }

        public new string this[string key]
        {
            get
            {
                if (TryGetValue(
                    key,
                    out var value
                ))
                {
                    return value;
                }
                else if (TryGetValue(
                    key.LowercaseFirstChar(),
                    out value
                ))
                {
                    return value;
                }

                return string.Empty;
            }
        }
    }
}
