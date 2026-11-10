namespace OptumLabResultsAPI.Models;

public sealed class LabResult
{
    public int Id { get; set; }
    public required string PatientId { get; set; }
    public required string TestName { get; set; }
    public required string ResultValue { get; set; }
    public required string Unit { get; set; }
    public DateTime ObservedDate { get; set; }
}
