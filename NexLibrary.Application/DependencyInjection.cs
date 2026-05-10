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

        return services;
    }
}