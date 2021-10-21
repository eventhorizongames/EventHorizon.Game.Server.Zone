namespace EventHorizon.Platform
{
    using System;

    using EventHorizon.Platform.Model;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public static class PlatformExtensions
    {
        public static IEndpointRouteBuilder MapPlatformDetails(
            this IEndpointRouteBuilder routes,
            Action<PlatformDetailsOptions> options
        )
        {
            var option = new PlatformDetailsOptions();

            options(option);

            var platformDetails = new PlatformDetailsModel(
                option
            );
            routes.MapGet(
                "/platform/details",
                async context => await context.Response.WriteAsJsonAsync(
                    platformDetails
                )
            );

            return routes;
        }
    }
}
