namespace EventHorizon.Game.I18n.Fetch
{
    using System.Collections.Generic;

    using MediatR;

    public struct FetchI18nMapForLocaleQuery : IRequest<IDictionary<string, string>>
    {
        public string Locale { get; set; }
        public FetchI18nMapForLocaleQuery(
            string locale
        )
        {
            Locale = locale;
        }
    }
}
