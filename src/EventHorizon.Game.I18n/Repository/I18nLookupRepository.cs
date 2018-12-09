using System.Collections.Generic;
using System.Linq;
using EventHorizon.Game.I18n.Model;

namespace EventHorizon.Game.I18n.Lookup
{
    public class I18nLookupRepository : I18nLookup, I18nResolver, I18nRepository
    {
        private Dictionary<string, Dictionary<string, string>> I18N_LOOKUP = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, string> GetRepository(string locale)
        {
            Dictionary<string, string> repository;
            if (I18N_LOOKUP.TryGetValue(locale, out repository))
            {
                return repository;
            }
            return new Dictionary<string, string>();
        }

        public string Lookup(string locale, string key)
        {
            Dictionary<string, string> localeTranslationDictionary;
            if (I18N_LOOKUP.TryGetValue(locale, out localeTranslationDictionary))
            {
                string translation;
                if (localeTranslationDictionary.TryGetValue(key, out translation))
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
        public string Resolve(string locale, string key, params I18nTokenValue[] tokenValueList)
        {
            // Resolve tokens in i18n translation with values passed in
            var translation = Lookup(locale, key);
            foreach (var tokenValue in tokenValueList)
            {
                translation = translation.Replace(
                    $"{{{tokenValue.Token}}}",
                    tokenValue.Value
                );
            }
            return translation;
        }

        public void SetRepository(string locale, Dictionary<string, string> i18nRepository)
        {
            I18N_LOOKUP[locale] = i18nRepository ?? new Dictionary<string, string>();
        }
    }
}