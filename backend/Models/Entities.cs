using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniversityManagement.Api.Models;

public static class Roles
{
    public const string Administration = "Administration";
    public const string Professor = "Professeur";
    public const string Student = "Etudiant";
}

[Index(nameof(Email), IsUnique = true)]
public class AppUser
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(150)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Role { get; set; } = Roles.Student;

    [MaxLength(10)]
    public string Avatar { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public StudentProfile? StudentProfile { get; set; }
    public ProfessorProfile? ProfessorProfile { get; set; }
    public ICollection<NotificationItem> Notifications { get; set; } = new List<NotificationItem>();
    public ICollection<ForumTopic> ForumTopics { get; set; } = new List<ForumTopic>();
    public ICollection<ForumMessage> ForumMessages { get; set; } = new List<ForumMessage>();
    public ICollection<LaboratoryReservation> Reservations { get; set; } = new List<LaboratoryReservation>();
}

[Index(nameof(Code), IsUnique = true)]
public class Establishment
{
    public int Id { get; set; }

    [MaxLength(180)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(120)]
    public string City { get; set; } = string.Empty;

    [MaxLength(220)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public ICollection<Department> Departments { get; set; } = new List<Department>();
}

[Index(nameof(Code), IsUnique = true)]
public class Department
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(150)]
    public string HeadName { get; set; } = string.Empty;

    public int EstablishmentId { get; set; }
    public Establishment? Establishment { get; set; }

    public ICollection<Filiere> Filieres { get; set; } = new List<Filiere>();
    public ICollection<ProfessorProfile> Professors { get; set; } = new List<ProfessorProfile>();
    public ICollection<Laboratory> Laboratories { get; set; } = new List<Laboratory>();
}

[Index(nameof(Code), IsUnique = true)]
public class Filiere
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(80)]
    public string Cycle { get; set; } = string.Empty;

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<StudentProfile> Students { get; set; } = new List<StudentProfile>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<TimetableEntry> TimetableEntries { get; set; } = new List<TimetableEntry>();
}

public class AcademicYear
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string Label { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

[Index(nameof(StudentNumber), IsUnique = true)]
[Index(nameof(Cne), IsUnique = true)]
public class StudentProfile
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    [MaxLength(40)]
    public string StudentNumber { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Cne { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    [MaxLength(80)]
    public string Level { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "Actif";

    public int FiliereId { get; set; }
    public Filiere? Filiere { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<GradeRecord> GradeRecords { get; set; } = new List<GradeRecord>();
}

public class ProfessorProfile
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    [MaxLength(100)]
    public string Grade { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Speciality { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Office { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "Actif";

    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<TimetableEntry> TimetableEntries { get; set; } = new List<TimetableEntry>();
    public ICollection<Laboratory> ResponsibleLabs { get; set; } = new List<Laboratory>();
}

[Index(nameof(Code), IsUnique = true)]
public class Laboratory
{
    public int Id { get; set; }

    [MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(250)]
    public string MainEquipment { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? ResponsibleProfessorId { get; set; }
    public ProfessorProfile? ResponsibleProfessor { get; set; }

    public ICollection<LaboratoryReservation> Reservations { get; set; } = new List<LaboratoryReservation>();
}

public class LaboratoryReservation
{
    public int Id { get; set; }

    public int LaboratoryId { get; set; }
    public Laboratory? Laboratory { get; set; }

    public int ReservedByUserId { get; set; }
    public AppUser? ReservedByUser { get; set; }

    public DateTime ReservationDate { get; set; }

    [MaxLength(80)]
    public string TimeSlot { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Purpose { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "Validée";
}

[Index(nameof(Code), IsUnique = true)]
public class Course
{
    public int Id { get; set; }

    [MaxLength(30)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int Credits { get; set; }

    [MaxLength(20)]
    public string Semester { get; set; } = "S1";

    public bool IsOnline { get; set; }

    public int FiliereId { get; set; }
    public Filiere? Filiere { get; set; }

    public int ProfessorId { get; set; }
    public ProfessorProfile? Professor { get; set; }

    public ICollection<CourseResource> Resources { get; set; } = new List<CourseResource>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<TimetableEntry> TimetableEntries { get; set; } = new List<TimetableEntry>();
    public ICollection<ForumTopic> ForumTopics { get; set; } = new List<ForumTopic>();
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<GradeRecord> GradeRecords { get; set; } = new List<GradeRecord>();
}

public class CourseResource
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Url { get; set; } = string.Empty;

    public bool Published { get; set; } = true;

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
}

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public StudentProfile? Student { get; set; }

    public int FiliereId { get; set; }
    public Filiere? Filiere { get; set; }

    public int AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }

    [MaxLength(30)]
    public string GroupName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "Inscrit";
}

public class TimetableEntry
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public int ProfessorId { get; set; }
    public ProfessorProfile? Professor { get; set; }

    public int FiliereId { get; set; }
    public Filiere? Filiere { get; set; }

    [MaxLength(20)]
    public string Day { get; set; } = string.Empty;

    [MaxLength(30)]
    public string TimeSlot { get; set; } = string.Empty;

    [MaxLength(60)]
    public string Room { get; set; } = string.Empty;

    [MaxLength(30)]
    public string GroupName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string SessionType { get; set; } = "Cours";
}

public class Assignment
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public int TotalPoints { get; set; }

    [MaxLength(30)]
    public string Status { get; set; } = "Ouvert";

    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}

public class AssignmentSubmission
{
    public int Id { get; set; }

    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }

    public int StudentId { get; set; }
    public StudentProfile? Student { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(250)]
    public string FileUrl { get; set; } = string.Empty;

    public int? Grade { get; set; }

    [MaxLength(250)]
    public string Feedback { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "Soumis";
}

public class Quiz
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    public int Questions { get; set; }

    public int DurationMinutes { get; set; }

    [MaxLength(30)]
    public string Availability { get; set; } = "Disponible";

    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}

public class QuizQuestion
{
    public int Id { get; set; }

    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    [MaxLength(500)]
    public string QuestionText { get; set; } = string.Empty;

    [MaxLength(250)]
    public string CorrectAnswer { get; set; } = string.Empty;

    public int Marks { get; set; }
}

public class ForumTopic
{
    public int Id { get; set; }

    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Forum { get; set; } = "Forum général";

    public int CreatedByUserId { get; set; }
    public AppUser? CreatedByUser { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsClosed { get; set; }

    public ICollection<ForumMessage> Messages { get; set; } = new List<ForumMessage>();
}

public class ForumMessage
{
    public int Id { get; set; }

    public int TopicId { get; set; }
    public ForumTopic? Topic { get; set; }

    public int CreatedByUserId { get; set; }
    public AppUser? CreatedByUser { get; set; }

    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AttendanceRecord
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public StudentProfile? Student { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public DateTime SessionDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Présent";
}

public class GradeRecord
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public StudentProfile? Student { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    [MaxLength(120)]
    public string Label { get; set; } = string.Empty;

    public decimal Score { get; set; }
    public decimal MaxScore { get; set; } = 20;

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}

public class NotificationItem
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser? User { get; set; }

    [MaxLength(160)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(400)]
    public string Message { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
