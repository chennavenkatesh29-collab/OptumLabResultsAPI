using OptumLabResultsAPI.DTOs;
using OptumLabResultsAPI.Models;
using OptumLabResultsAPI.Repositories;

namespace OptumLabResultsAPI.Services;

public sealed class LabResultService(ILabResultRepository repository) : ILabResultService
{
    public Task<LabResult> CreateAsync(CreateLabResultRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateObservedDate(request.ObservedDate);

        var result = new LabResult
        {
            PatientId = request.PatientId.Trim(),
            TestName = request.TestName.Trim(),
            ResultValue = request.ResultValue.Trim(),
            Unit = request.Unit.Trim(),
            ObservedDate = request.ObservedDate
        };

        return repository.AddAsync(result, cancellationToken);
    }

    public Task<LabResult?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
        return repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<LabResult>> GetAsync(string? patientId, CancellationToken cancellationToken = default)
    {
        if (patientId is null || string.IsNullOrWhiteSpace(patientId))
            return await repository.GetAllAsync(cancellationToken);

        return await repository.GetByPatientIdAsync(patientId.Trim(), cancellationToken);
    }

    public async Task<LabResult?> UpdateAsync(int id, UpdateLabResultRequest request, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
        ArgumentNullException.ThrowIfNull(request);
        ValidateObservedDate(request.ObservedDate);

        var existing = await repository.GetByIdAsync(id, cancellationToken);
        if (existing is null) return null;

        existing.PatientId = request.PatientId.Trim();
        existing.TestName = request.TestName.Trim();
        existing.ResultValue = request.ResultValue.Trim();
        existing.Unit = request.Unit.Trim();
        existing.ObservedDate = request.ObservedDate;

        await repository.UpdateAsync(existing, cancellationToken);
        return existing;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");
        return repository.DeleteAsync(id, cancellationToken);
    }

    private static void ValidateObservedDate(DateTime observedDate)
    {
        if (observedDate == default)
            throw new ArgumentException("ObservedDate is required.", nameof(observedDate));

        if (observedDate > DateTime.UtcNow.AddDays(1))
            throw new ArgumentException("ObservedDate cannot be in the future.", nameof(observedDate));
    }
}
