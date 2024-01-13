namespace EventHorizon.Game.I18n.Lookup;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using EventHorizon.Game.I18n.Model;

public class I18nLookupRepository
    : I18nLookup,
    I18nResolver,
    I18nRepository
{
    private readonly IDictionary<string, string> _defaultLookup = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
    private readonly Dictionary<string, IDictionary<string, string>> _i18nLookup = new();

    public IDictionary<string, string> GetRepository(
        string locale
    )
    {
        if (_i18nLookup.TryGetValue(
            locale,
            out var repository
        ))
        {
            return repository;
        }
        return new Dictionary<string, string>();
    }

    public string Lookup(
        string locale,
        string key
    )
    {
        if (_i18nLookup.TryGetValue(
            locale,
            out var localeTranslationDictionary
        ))
        {
            if (key == null)
            {
                key = "''";
            }
            if (localeTranslationDictionary.TryGetValue(
                key,
                out var translation
            ))
            {
                return translation;
            }
        }
        return $"[[{key} (NOT_FOUND)]]";
    }
    /// <summary>
    /// 
    /// Example Translation: Hello, {userFirstName}, how are you on this {dayOfWeek}
    /// keyValues: [{ "key": "userFirstName", "value": "User" }, { "key": "dayOfWeek", "value": "Monday" }]
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="tokenValueList"></param>
    /// <returns></returns>
    public string Resolve(
        string locale,
        string key,
        params I18nTokenValue[] tokenValueList
    )
    {
        // Resolve tokens in i18n translation with values passed in
        var translation = Lookup(
            locale,
            key
        );
        foreach (var tokenValue in tokenValueList)
        {
            translation = translation.Replace(
                $"${{{tokenValue.Token}}}",
                tokenValue.Value
            );
        }
        return translation;
    }

    public void SetRepository(
        string locale,
        IDictionary<string, string> i18nRepository
    )
    {
        if (_i18nLookup.ContainsKey(locale) && i18nRepository != null)
        {
            var existing = _i18nLookup[locale];
            // Merge the passed into existing
            _i18nLookup[locale] = existing.Concat(i18nRepository)
                .GroupBy(
                    item => item.Key,
                    item => item.Value
                )
                .ToDictionary(
                    item => item.Key,
                    item => item.Last()
                );
            return;
        }

        _i18nLookup[locale] = i18nRepository ?? _defaultLookup;
    }
}
