namespace EventHorizon.Game.I18n
{
    using System.Collections.Generic;

    public interface I18nRepository
    {
        IDictionary<string, string> GetRepository(
            string locale
        );
        void SetRepository(
            string locale,
            IDictionary<string, string> i18nRepository
        );
    }
}
