﻿namespace EventHorizon.Zone.System.Wizard.Api
{
    using EventHorizon.Zone.System.Wizard.Model;
    using global::System.Collections.Generic;

    public interface WizardRepository
    {
        IEnumerable<WizardMetadata> All { get; }

        void Clear();

        void Set(
            string id,
            WizardMetadata wizard
        );
    }
}
