namespace NexLibrary.Application.Models;

public sealed class CurrentRequestInfo
{
    public string? IpAdresi { get; set; }

    public string? UserAgent { get; set; }

    public string? MacAdresi { get; set; }

    public string? CihazBilgisi { get; set; }

    public string? TarayiciBilgisi { get; set; }

    public string? IsletimSistemi { get; set; }

    public string? Dil { get; set; }

    public string? Referer { get; set; }

    public string? Host { get; set; }

    public string? HttpMethod { get; set; }

    public string? RequestPath { get; set; }

    public string? HeaderJson { get; set; }
}