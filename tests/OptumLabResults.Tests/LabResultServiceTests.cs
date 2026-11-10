using OptumLabResultsAPI.DTOs;
using OptumLabResultsAPI.Repositories;
using OptumLabResultsAPI.Services;
using Xunit;

namespace OptumLabResults.Tests;

public sealed class LabResultServiceTests
{
    private readonly InMemoryLabResultRepository _repository = new();
    private readonly LabResultService _service;

    public LabResultServiceTests()
    {
        _service = new LabResultService(_repository);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateLabResultWithGeneratedId()
    {
        var request = new CreateLabResultRequest
        {
            PatientId = "PAT-1001",
            TestName = "Hemoglobin",
            ResultValue = "13.5",
            Unit = "g/dL",
            ObservedDate = DateTime.UtcNow.AddDays(-1)
        };

        var created = await _service.CreateAsync(request);

        Assert.True(created.Id > 0);
        Assert.Equal("PAT-1001", created.PatientId);
        Assert.Equal("Hemoglobin", created.TestName);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOnlyMatchingPatientResults()
    {
        await _service.CreateAsync(new CreateLabResultRequest
        {
            PatientId = "PAT-1001", TestName = "Hemoglobin", ResultValue = "13.5",
            Unit = "g/dL", ObservedDate = DateTime.UtcNow.AddDays(-1)
        });
        await _service.CreateAsync(new CreateLabResultRequest
        {
            PatientId = "PAT-2002", TestName = "Glucose", ResultValue = "95",
            Unit = "mg/dL", ObservedDate = DateTime.UtcNow.AddDays(-1)
        });

        var results = await _service.GetAsync("PAT-1001");

        var result = Assert.Single(results);
        Assert.Equal("PAT-1001", result.PatientId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenRecordDoesNotExist()
    {
        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveExistingRecord()
    {
        var created = await _service.CreateAsync(new CreateLabResultRequest
        {
            PatientId = "PAT-1001", TestName = "Glucose", ResultValue = "95",
            Unit = "mg/dL", ObservedDate = DateTime.UtcNow.AddDays(-1)
        });

        var deleted = await _service.DeleteAsync(created.Id);
        var result = await _service.GetByIdAsync(created.Id);

        Assert.True(deleted);
        Assert.Null(result);
    }
}
