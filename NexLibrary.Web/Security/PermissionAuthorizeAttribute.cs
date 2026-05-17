using Microsoft.AspNetCore.Mvc;

namespace NexLibrary.Web.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class PermissionAuthorizeAttribute : TypeFilterAttribute
{
    public PermissionAuthorizeAttribute(string permissionCode)
        : base(typeof(PermissionAuthorizationFilter))
    {
        Arguments = new object[] { permissionCode };
    }
}