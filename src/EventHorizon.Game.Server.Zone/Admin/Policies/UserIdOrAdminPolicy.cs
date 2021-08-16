namespace EventHorizon.Game.Server.Zone.Admin.Policies
{
    using System;

    using Microsoft.AspNetCore.Authorization;

    public static class UserIdOrAdminPolicy
    {
        private const string PolicyName = "UserIdOrAdmin";
        private const string SubjectClaimType = "sub";
        private const string RoleClaimType = "role";
        private const string AdminRoleName = "Admin";
        private const string InvalidOwnerUserId = "<invalid>";

        public static AuthorizationOptions AddUserIdOrAdminPolicy(
            this AuthorizationOptions options,
            string ownerUserId
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
                    context => context.User.HasClaim(
                        SubjectClaimType,
                        ownerUserId
                    ) || context.User.HasClaim(
                        RoleClaimType,
                        AdminRoleName
                    )
                )
            );

            return options;
        }
    }
}
