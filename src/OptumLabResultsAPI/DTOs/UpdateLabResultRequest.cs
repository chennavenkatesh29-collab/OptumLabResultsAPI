using System.ComponentModel.DataAnnotations;

namespace OptumLabResultsAPI.DTOs;

public sealed class UpdateLabResultRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string PatientId { get; init; } = string.Empty;

    [Required, StringLength(150, MinimumLength = 1)]
    public string TestName { get; init; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 1)]
    public string ResultValue { get; init; } = string.Empty;

    [Required, StringLength(30, MinimumLength = 1)]
    public string Unit { get; init; } = string.Empty;

    public DateTime ObservedDate { get; init; }
}
