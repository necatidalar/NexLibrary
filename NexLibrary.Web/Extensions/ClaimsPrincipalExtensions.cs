using System.Security.Claims;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool HasPermission(
        this ClaimsPrincipal user,
        string permissionCode)
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        return user.HasClaim(
            AppClaimTypes.Permission,
            permissionCode);
    }
}