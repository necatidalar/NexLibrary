using Microsoft.Extensions.DependencyInjection;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Application.Services;

namespace NexLibrary.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IFormFieldService, FormFieldService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<ILoanService, LoanService>();
        services.AddScoped<IBookCopyService, BookCopyService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IApiClientAuthService, ApiClientAuthService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<ICurrentRequestInfoService, NullCurrentRequestInfoService>();

        return services;
    }
}