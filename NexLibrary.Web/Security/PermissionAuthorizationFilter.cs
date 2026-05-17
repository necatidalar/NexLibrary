using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Web.Security;

public sealed class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly string _permissionCode;

    public PermissionAuthorizationFilter(string permissionCode)
    {
        _permissionCode = permissionCode;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new ChallengeResult();
            return Task.CompletedTask;
        }

        var hasPermission = user.HasClaim(
            AppClaimTypes.Permission,
            _permissionCode);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}