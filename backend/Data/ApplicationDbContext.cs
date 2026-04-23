using Microsoft.EntityFrameworkCore;
using UniversityManagement.Api.Models;

namespace UniversityManagement.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Establishment> Establishments => Set<Establishment>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Filiere> Filieres => Set<Filiere>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<StudentProfile> Students => Set<StudentProfile>();
    public DbSet<ProfessorProfile> Professors => Set<ProfessorProfile>();
    public DbSet<Laboratory> Laboratories => Set<Laboratory>();
    public DbSet<LaboratoryReservation> LaboratoryReservations => Set<LaboratoryReservation>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseResource> CourseResources => Set<CourseResource>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<TimetableEntry> TimetableEntries => Set<TimetableEntry>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<ForumTopic> ForumTopics => Set<ForumTopic>();
    public DbSet<ForumMessage> ForumMessages => Set<ForumMessage>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<GradeRecord> GradeRecords => Set<GradeRecord>();
    public DbSet<NotificationItem> Notifications => Set<NotificationItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentProfile>()
            .HasOne(x => x.User)
            .WithOne(x => x.StudentProfile)
            .HasForeignKey<StudentProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfessorProfile>()
            .HasOne(x => x.User)
            .WithOne(x => x.ProfessorProfile)
            .HasForeignKey<ProfessorProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Laboratory>()
            .HasOne(x => x.ResponsibleProfessor)
            .WithMany(x => x.ResponsibleLabs)
            .HasForeignKey(x => x.ResponsibleProfessorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<GradeRecord>()
            .Property(x => x.Score)
            .HasPrecision(5, 2);

        modelBuilder.Entity<GradeRecord>()
            .Property(x => x.MaxScore)
            .HasPrecision(5, 2);

        modelBuilder.Entity<AppUser>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<NotificationItem>()
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
