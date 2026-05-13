using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexLibrary.Desktop.Forms;
using NexLibrary.Desktop.Options;
using NexLibrary.Desktop.Services;

namespace NexLibrary.Desktop;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();

        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();

        var mainForm = serviceProvider.GetRequiredService<FrmMain>();

        Application.Run(mainForm);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var apiSettings = new ApiSettings();
        configuration.GetSection("ApiSettings").Bind(apiSettings);

        if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
        {
            throw new InvalidOperationException("ApiSettings:BaseUrl ayarı bulunamadı.");
        }

        services.AddSingleton(apiSettings);

        services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri(apiSettings.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddScoped<FormFieldApiService>();
        services.AddScoped<BookApiService>();
        services.AddScoped<MemberApiService>();
        services.AddScoped<LoanApiService>();
        services.AddScoped<BookCopyApiService>();

        services.AddTransient<FrmMain>();
        services.AddTransient<FrmBooks>();
        services.AddTransient<FrmMembers>();
        services.AddTransient<FrmLoans>();
        services.AddTransient<FrmBookCopies>();
        services.AddTransient<FrmFormFields>();
        services.AddTransient<FrmListSettings>();
    }
}