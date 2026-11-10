using System.Collections.Concurrent;
using OptumLabResultsAPI.Models;

namespace OptumLabResultsAPI.Repositories;

/// <summary>
/// Demo-only in-memory storage. Data is lost when the application restarts.
/// Replace this implementation with EF Core or Dapper for persistent storage.
/// </summary>
public sealed class InMemoryLabResultRepository : ILabResultRepository
{
    private readonly ConcurrentDictionary<int, LabResult> _results = new();
    private int _nextId;

    public Task<LabResult> AddAsync(LabResult result, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var stored = Copy(result);
        stored.Id = Interlocked.Increment(ref _nextId);
        _results[stored.Id] = stored;
        return Task.FromResult(Copy(stored));
    }

    public Task<LabResult?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_results.TryGetValue(id, out var result) ? Copy(result) : null);
    }

    public Task<IReadOnlyList<LabResult>> GetByPatientIdAsync(
        string patientId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyList<LabResult> results = _results.Values
            .Where(x => string.Equals(x.PatientId, patientId, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.ObservedDate)
            .Select(Copy)
            .ToList();
        return Task.FromResult(results);
    }

    public Task<IReadOnlyList<LabResult>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyList<LabResult> results = _results.Values
            .OrderByDescending(x => x.ObservedDate)
            .Select(Copy)
            .ToList();
        return Task.FromResult(results);
    }

    public Task<bool> UpdateAsync(LabResult result, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_results.ContainsKey(result.Id))
            return Task.FromResult(false);

        _results[result.Id] = Copy(result);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_results.TryRemove(id, out _));
    }

    private static LabResult Copy(LabResult source) => new()
    {
        Id = source.Id,
        PatientId = source.PatientId,
        TestName = source.TestName,
        ResultValue = source.ResultValue,
        Unit = source.Unit,
        ObservedDate = source.ObservedDate
    };
}
