using System.Collections.Generic;

namespace EventHorizon.Game.I18n
{
    public interface I18nRepository
    {
        Dictionary<string, string> GetRepository(string locale);
        void SetRepository(string locale, Dictionary<string, string> I18nRepository);
    }
}