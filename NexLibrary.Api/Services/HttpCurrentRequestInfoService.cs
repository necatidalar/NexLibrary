using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Application.Models;

namespace NexLibrary.Api.Services;

public sealed class HttpCurrentRequestInfoService : ICurrentRequestInfoService
{
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization",
        "Cookie",
        "Set-Cookie",
        "X-Api-Key"
    };

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentRequestInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentRequestInfo GetCurrent()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
        {
            return new CurrentRequestInfo();
        }

        var request = context.Request;

        var userAgent = GetHeader("User-Agent");
        var secChUa = GetHeader("sec-ch-ua");
        var secChUaMobile = GetHeader("sec-ch-ua-mobile");
        var secChUaPlatform = GetHeader("sec-ch-ua-platform");

        return new CurrentRequestInfo
        {
            IpAdresi = GetIpAddress(context),
            UserAgent = Truncate(userAgent, 1000),
            MacAdresi = Truncate(GetHeader("X-Client-Mac"), 100),
            CihazBilgisi = Truncate(GetDeviceInfo(secChUaMobile, userAgent), 250),
            TarayiciBilgisi = Truncate(string.IsNullOrWhiteSpace(secChUa) ? userAgent : secChUa, 500),
            IsletimSistemi = Truncate(GetOperatingSystem(secChUaPlatform, userAgent), 250),
            Dil = Truncate(GetHeader("Accept-Language"), 250),
            Referer = Truncate(GetHeader("Referer"), 1000),
            Host = Truncate(request.Host.Value, 250),
            HttpMethod = Truncate(request.Method, 20),
            RequestPath = Truncate($"{request.Path}{request.QueryString}", 1000),
            HeaderJson = GetSafeHeaderJson(context)
        };
    }

    private string? GetHeader(string key)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
        {
            return null;
        }

        return context.Request.Headers.TryGetValue(key, out var value)
            ? value.ToString()
            : null;
    }

    private static string? GetIpAddress(HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();

        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            return forwardedFor
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault();
        }

        var realIp = context.Request.Headers["X-Real-IP"].ToString();

        if (!string.IsNullOrWhiteSpace(realIp))
        {
            return realIp;
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    private static string GetDeviceInfo(string? secChUaMobile, string? userAgent)
    {
        if (string.Equals(secChUaMobile, "?1", StringComparison.OrdinalIgnoreCase))
        {
            return "Mobile";
        }

        if (string.Equals(secChUaMobile, "?0", StringComparison.OrdinalIgnoreCase))
        {
            return "Desktop";
        }

        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return "Bilinmiyor";
        }

        if (userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase) ||
            userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase) ||
            userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
        {
            return "Mobile";
        }

        return "Desktop";
    }

    private static string GetOperatingSystem(string? secChUaPlatform, string? userAgent)
    {
        if (!string.IsNullOrWhiteSpace(secChUaPlatform))
        {
            return secChUaPlatform.Replace("\"", string.Empty).Trim();
        }

        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return "Bilinmiyor";
        }

        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
        {
            return "Windows";
        }

        if (userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase) ||
            userAgent.Contains("Macintosh", StringComparison.OrdinalIgnoreCase))
        {
            return "macOS";
        }

        if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            return "Android";
        }

        if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) ||
            userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase))
        {
            return "iOS";
        }

        if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
        {
            return "Linux";
        }

        return "Bilinmiyor";
    }

    private static string? GetSafeHeaderJson(HttpContext context)
    {
        var headers = context.Request.Headers
            .Where(x => !SensitiveHeaders.Contains(x.Key))
            .ToDictionary(
                x => x.Key,
                x => x.Value.ToString());

        return JsonSerializer.Serialize(headers);
    }

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        return trimmed.Length <= maxLength
            ? trimmed
            : trimmed[..maxLength];
    }
}