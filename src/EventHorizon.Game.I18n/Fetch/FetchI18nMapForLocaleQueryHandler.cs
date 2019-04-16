using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Extensions;
using MediatR;

namespace EventHorizon.Game.I18n.Fetch
{
    public struct FetchI18nMapForLocaleQueryHandler : IRequestHandler<FetchI18nMapForLocaleQuery, IDictionary<string, string>>
    {
        private static string DEFAULT_LOCALE = "default";

        readonly I18nRepository _i18nRepo;
        public FetchI18nMapForLocaleQueryHandler(
            I18nRepository i18nRepo
        )
        {
            _i18nRepo = i18nRepo;
        }
        public Task<IDictionary<string, string>> Handle(
            FetchI18nMapForLocaleQuery request,
            CancellationToken cancellationToken
        )
        {
            var i18nRepo = _i18nRepo.GetRepository(
                request.Locale ?? DEFAULT_LOCALE
            );
            if (i18nRepo.IsEmpty())
            {
                i18nRepo = _i18nRepo.GetRepository(
                    request.Locale ?? DEFAULT_LOCALE
                );
            }
            return Task.FromResult(
                i18nRepo
            );
        }
    }
}