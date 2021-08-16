namespace EventHorizon.Game.I18n.Lookup
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using EventHorizon.Game.I18n.Model;

    public class I18nLookupRepository : I18nLookup, I18nResolver, I18nRepository
    {
        private static readonly IDictionary<string, string> DEFAULT_LOOKUP = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        private Dictionary<string, IDictionary<string, string>> I18N_LOOKUP = new Dictionary<string, IDictionary<string, string>>();

        public IDictionary<string, string> GetRepository(
            string locale
        )
        {
            IDictionary<string, string> repository;
            if (I18N_LOOKUP.TryGetValue(
                locale, out repository
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
            IDictionary<string, string> localeTranslationDictionary;
            if (I18N_LOOKUP.TryGetValue(
                locale,
                out localeTranslationDictionary
            ))
            {
                string translation;
                if (key == null)
                {
                    key = "";
                }
                if (localeTranslationDictionary.TryGetValue(
                    key,
                    out translation
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
            var translation = this.Lookup(
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
            if (I18N_LOOKUP.ContainsKey(locale) && i18nRepository != null)
            {
                var existing = I18N_LOOKUP[locale];
                // Merge the passed into existing
                I18N_LOOKUP[locale] = existing.Concat(i18nRepository)
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

            I18N_LOOKUP[locale] = i18nRepository ?? DEFAULT_LOOKUP;
        }
    }
}
