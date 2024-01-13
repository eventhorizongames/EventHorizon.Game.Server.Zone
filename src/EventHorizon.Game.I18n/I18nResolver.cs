namespace EventHorizon.Game.I18n;

using EventHorizon.Game.I18n.Model;

public interface I18nResolver
{
    string Resolve(string locale, string key, params I18nTokenValue[] keyValues);
}
