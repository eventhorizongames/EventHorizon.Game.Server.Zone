namespace EventHorizon.Game.Server.Zone.Controllers
{
    using System;

    using IdentityModel.AspNetCore.OAuth2Introspection;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    public class WebSocketTokenRetriever
    {
        static Func<HttpRequest, string> AuthHeaderTokenRetriever { get; set; }
        static Func<HttpRequest, string> QueryStringTokenRetriever { get; set; }

        static WebSocketTokenRetriever()
        {
            AuthHeaderTokenRetriever = TokenRetrieval.FromAuthorizationHeader();
            QueryStringTokenRetriever = TokenRetrieval.FromQueryString();
        }

        public static string FromHeaderAndQueryString(HttpRequest request)
        {
            var token = AuthHeaderTokenRetriever(request);

            if (string.IsNullOrEmpty(token))
            {
                token = QueryStringTokenRetriever(request);
            }

            return token;
        }
    }
}
