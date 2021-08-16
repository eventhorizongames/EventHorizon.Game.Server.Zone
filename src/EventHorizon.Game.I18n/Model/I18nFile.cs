using System.Collections.Generic;

namespace EventHorizon.Game.I18n.Model
{
    public struct I18nFile
    {
        public string Locale { get; set; }
        public Dictionary<string, string> TranslationList { get; set; }
    }
}
