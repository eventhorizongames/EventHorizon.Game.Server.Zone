namespace EventHorizon.Game.Server.Zone.Admin.Policies;

using System.Linq;

using Microsoft.AspNetCore.Authorization;

public static class UserIdOrClientIdOrAdminPolicy
{
    public const string PolicyName = "UserIdOrClientIdOrAdmin";

    private const string SubjectClaimType = "sub";
    private const string RoleClaimType = "role";
    private const string ClientIdClaimType = "client_id";
    private const string AdminRoleName = "Admin";
    private const string InvalidOwnerUserId = "<invalid>";

    public static AuthorizationOptions AddUserIdOrClientIdOrAdminPolicy(
        this AuthorizationOptions options,
        string ownerUserId,
        string platformId
    )
    {
        if (string.IsNullOrWhiteSpace(
            ownerUserId
        ))
        {
            ownerUserId = InvalidOwnerUserId;
        }
        options.AddPolicy(
            PolicyName,
            configure => configure.RequireAssertion(
                // User Id Check
                context => context.User.HasClaim(
                    SubjectClaimType,
                    ownerUserId
                )
                // Client Id Check
                || context.User.Claims.Any(
                    a => a.Type == ClientIdClaimType
                        && a.Value.EndsWith(
                            platformId
                        )
                )
                // Admin Role Check
                || context.User.HasClaim(
                    RoleClaimType,
                    AdminRoleName
                )
            )
        );

        return options;
    }
}
