-- Optum Grade 26 take-home assignment: LabResults table and example queries.
-- SQL Server

CREATE TABLE dbo.LabResults
(
    Id           INT IDENTITY(1,1) NOT NULL
        CONSTRAINT PK_LabResults PRIMARY KEY,
    PatientId    NVARCHAR(100) NOT NULL,
    TestName     NVARCHAR(150) NOT NULL,
    ResultValue  NVARCHAR(100) NOT NULL,
    Unit         NVARCHAR(30) NOT NULL,
    ObservedDate DATETIME2(0) NOT NULL,
    CreatedAtUtc DATETIME2(0) NOT NULL
        CONSTRAINT DF_LabResults_CreatedAtUtc DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_LabResults_PatientId_ObservedDate
    ON dbo.LabResults (PatientId, ObservedDate DESC);

-- Example 1: Filtering - retrieve one patient's results within a date range.
DECLARE @PatientId NVARCHAR(100) = N'PAT-1001';
DECLARE @FromDate DATETIME2(0) = '2026-01-01';
DECLARE @ToDate   DATETIME2(0) = '2026-12-31';

SELECT Id, PatientId, TestName, ResultValue, Unit, ObservedDate
FROM dbo.LabResults
WHERE PatientId = @PatientId
  AND ObservedDate >= @FromDate
  AND ObservedDate < DATEADD(DAY, 1, @ToDate)
ORDER BY ObservedDate DESC;

-- Example 2: Aggregation - count tests per patient.
SELECT PatientId, COUNT(*) AS TotalTests
FROM dbo.LabResults
GROUP BY PatientId
ORDER BY TotalTests DESC;
