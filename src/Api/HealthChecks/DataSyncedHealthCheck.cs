using Api.Models;
using Api.Persistence;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.HealthChecks;

public sealed class DataSyncedHealthCheck : IHealthCheck
{
    private readonly IRepository _repository;

    public DataSyncedHealthCheck(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var synced = await _repository.QueryableOf<SyncHistory>().AnyAsync();
            return synced ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, description: ex.Message, exception: ex);
        }
    }
}
