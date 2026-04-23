namespace UniversityManagement.Api.Models;

public record LoginRequest(string Email, string Password);

public record StudentUpsertDto(
    string FullName,
    string Email,
    string? Password,
    string? Avatar,
    string StudentNumber,
    string Cne,
    string Gender,
    string Level,
    string Status,
    int FiliereId,
    DateTime? EnrollmentDate
);

public record ProfessorUpsertDto(
    string FullName,
    string Email,
    string? Password,
    string? Avatar,
    int DepartmentId,
    string Grade,
    string Speciality,
    string Office,
    string Status
);
