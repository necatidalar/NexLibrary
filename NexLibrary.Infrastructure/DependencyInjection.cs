using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexLibrary.Infrastructure.Persistence;

namespace NexLibrary.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection connection string bulunamadı.");
        }

        services.AddDbContext<NexLibraryDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }
}