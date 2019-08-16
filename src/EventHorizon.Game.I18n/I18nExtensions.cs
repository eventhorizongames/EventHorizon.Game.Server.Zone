
using EventHorizon.Game.I18n.Loader;
using EventHorizon.Game.I18n.Lookup;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.I18n
{
    public static class I18nExtensions
    {
        public static IServiceCollection AddI18n(this IServiceCollection services)
        {
            var i18nLookupRepository = new I18nLookupRepository();
            return services
                .AddSingleton<I18nLookup>(i18nLookupRepository)
                .AddSingleton<I18nRepository>(i18nLookupRepository)
                .AddSingleton<I18nResolver>(i18nLookupRepository)
            ;
        }
        public static void UseI18n(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new I18nLoadEvent()).GetAwaiter().GetResult();
            }
        }
    }
}