using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using NexLibrary.Web.Options;
using NexLibrary.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

var apiSettings = new ApiSettings();

builder.Configuration
    .GetSection("ApiSettings")
    .Bind(apiSettings);

if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
{
    throw new InvalidOperationException("ApiSettings:BaseUrl ayarı bulunamadı.");
}

builder.Services.AddSingleton(apiSettings);

void ConfigureApiClient(HttpClient client)
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(30);
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthTokenHandler>();

builder.Services.AddHttpClient<BookApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<FormFieldApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<DashboardApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<MemberApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<LoanApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<BookCopyApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<UserApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<PermissionApiService>(ConfigureApiClient)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<AuthApiService>(ConfigureApiClient);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "NexLibrary.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();