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

builder.Services.AddHttpClient<BookApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<FormFieldApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<DashboardApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<MemberApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<LoanApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<BookCopyApiService>(ConfigureApiClient);

builder.Services.AddHttpClient<UserApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<AuthApiService>(ConfigureApiClient);
builder.Services.AddHttpClient<PermissionApiService>(ConfigureApiClient);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "NexLibrary.Auth";
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