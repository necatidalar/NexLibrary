using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Application.Models;

namespace NexLibrary.Application.Services;

public sealed class NullCurrentRequestInfoService : ICurrentRequestInfoService
{
    public CurrentRequestInfo GetCurrent()
    {
        return new CurrentRequestInfo();
    }
}