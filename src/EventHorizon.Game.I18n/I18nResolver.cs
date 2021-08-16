using EventHorizon.Game.I18n.Model;

namespace EventHorizon.Game.I18n
{
    public interface I18nResolver
    {
        string Resolve(string locale, string key, params I18nTokenValue[] keyValues);
    }
}
