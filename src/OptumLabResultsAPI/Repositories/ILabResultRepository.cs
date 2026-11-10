using OptumLabResultsAPI.Models;

namespace OptumLabResultsAPI.Repositories;

public interface ILabResultRepository
{
    Task<LabResult> AddAsync(LabResult result, CancellationToken cancellationToken = default);
    Task<LabResult?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LabResult>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LabResult>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(LabResult result, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
