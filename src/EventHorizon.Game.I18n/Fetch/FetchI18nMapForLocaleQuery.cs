using System.Collections.Generic;
using MediatR;

namespace EventHorizon.Game.I18n.Fetch
{
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