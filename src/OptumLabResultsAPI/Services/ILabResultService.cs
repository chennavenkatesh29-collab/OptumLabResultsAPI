using OptumLabResultsAPI.DTOs;
using OptumLabResultsAPI.Models;

namespace OptumLabResultsAPI.Services;

public interface ILabResultService
{
    Task<LabResult> CreateAsync(CreateLabResultRequest request, CancellationToken cancellationToken = default);
    Task<LabResult?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LabResult>> GetAsync(string? patientId, CancellationToken cancellationToken = default);
    Task<LabResult?> UpdateAsync(int id, UpdateLabResultRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
