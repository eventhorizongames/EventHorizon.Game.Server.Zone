namespace EventHorizon.Zone.System.ArtifactManagement.Query;
using EventHorizon.Zone.System.ArtifactManagement.Model;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class IsNotValidArtifactUrlDomainHandler
    : IRequestHandler<IsNotValidArtifactUrlDomain, bool>
{
    private readonly ArtifactManagementSystemSettings _settings;

    public IsNotValidArtifactUrlDomainHandler(
        ArtifactManagementSystemSettings settings
    )
    {
        _settings = settings;
    }

    public Task<bool> Handle(
        IsNotValidArtifactUrlDomain request,
        CancellationToken cancellationToken
    )
    {
        if (Uri.TryCreate(
            request.ArtifactUrl,
            new UriCreationOptions(),
            out var uri
        ))
        {
            var uriDomain = uri.Host;
            if (uriDomain.IsNullOrEmpty())
            {
                return true.FromResult();
            }
            else if (_settings.AllowedDomainList.Contains(
                ArtifactManagementSystemContants.ALL_DOMAINS
            ))
            {
                return false.FromResult();
            }

            foreach (var allowedDomain in _settings.AllowedDomainList)
            {
                if (uriDomain.ToLowerInvariant().Equals(
                    allowedDomain.ToLowerInvariant()
                ))
                {
                    return false.FromResult();
                }
            }
        }

        return true.FromResult();
    }
}
