namespace EventHorizon.Game.I18n.Fetch
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    public class FetchI18nMapForLocaleQueryHandler
        : IRequestHandler<FetchI18nMapForLocaleQuery, IDictionary<string, string>>
    {
        private static readonly string DEFAULT_LOCALE = "default";

        private readonly I18nRepository _i18nRepo;

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
            if (IsEmpty(
                i18nRepo
            ))
            {
                i18nRepo = _i18nRepo.GetRepository(
                    DEFAULT_LOCALE
                );
            }
            return Task.FromResult(
                i18nRepo
            );
        }

        public bool IsEmpty<TSource>(
            IEnumerable<TSource> source
        )
        {
            return !source?.Any() ?? true;
        }
    }
}
