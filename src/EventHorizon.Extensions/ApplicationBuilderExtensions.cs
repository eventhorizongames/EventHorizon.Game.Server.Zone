namespace Microsoft.AspNetCore.Builder
{

    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

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

        public static IApplicationBuilder SendMediatorCommand<T>(
            this IApplicationBuilder applicationBuilder,
            T command
        )
        {
            using var scope = applicationBuilder.CreateServiceScope();
            scope.ServiceProvider.GetService<IMediator>().Send(
                command
            ).ConfigureAwait(false).GetAwaiter().GetResult();

            return applicationBuilder;
        }
    }
}