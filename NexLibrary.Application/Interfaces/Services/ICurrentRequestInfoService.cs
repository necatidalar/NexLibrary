using NexLibrary.Application.Models;

namespace NexLibrary.Application.Interfaces.Services;

public interface ICurrentRequestInfoService
{
    CurrentRequestInfo GetCurrent();
}