
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IServiceScope CreateServiceScope(
            this IApplicationBuilder applicationBuilder
        )
        {
            return applicationBuilder
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope();
        }
    }
}