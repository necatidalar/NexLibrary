using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Dashboard;

namespace NexLibrary.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<ApiResponse<DashboardSummaryResponse>> GetSummaryAsync(
        CancellationToken cancellationToken = default);
}