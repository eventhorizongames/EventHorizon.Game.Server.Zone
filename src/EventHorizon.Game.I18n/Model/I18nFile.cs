namespace EventHorizon.Game.I18n.Model
{
    using System.Collections.Generic;

    public struct I18nFile
    {
        public string Locale { get; set; }
        public Dictionary<string, string> TranslationList { get; set; }
    }
}
