using Microsoft.EntityFrameworkCore;
using UniversityManagement.Api.Data;
using UniversityManagement.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "server=localhost;port=3306;database=university_management_db;user=root;password=;TreatTinyAsBoolean=true;AllowUserVariables=true;CharSet=utf8mb4;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("frontend");

var api = app.MapGroup("/api");

api.MapGet("/health", () => Results.Ok(new { status = "ok", app = "UniversityManagement.Api", database = "MySQL / XAMPP" }));

api.MapGet("/dashboard/stats", async (ApplicationDbContext db) =>
    Results.Ok(new
    {
        establishments = await db.Establishments.CountAsync(),
        departments = await db.Departments.CountAsync(),
        filieres = await db.Filieres.CountAsync(),
        students = await db.Students.CountAsync(),
        professors = await db.Professors.CountAsync(),
        laboratories = await db.Laboratories.CountAsync(),
        courses = await db.Courses.CountAsync(),
        timetable = await db.TimetableEntries.CountAsync(),
        resources = await db.CourseResources.CountAsync(),
        assignments = await db.Assignments.CountAsync(),
        quizzes = await db.Quizzes.CountAsync(),
        forums = await db.ForumTopics.CountAsync(),
        notifications = await db.Notifications.CountAsync()
    }));

api.MapGet("/lookups", async (ApplicationDbContext db) =>
{
    var establishments = await db.Establishments.OrderBy(x => x.Name).Select(x => new { id = x.Id, value = x.Id, label = x.Name, name = x.Name }).ToListAsync();
    var departments = await db.Departments.OrderBy(x => x.Name).Select(x => new { id = x.Id, value = x.Id, label = x.Name, name = x.Name }).ToListAsync();
    var filieres = await db.Filieres.OrderBy(x => x.Name).Select(x => new { id = x.Id, value = x.Id, label = x.Name, name = x.Name }).ToListAsync();
    var professors = await db.Professors.Include(x => x.User).OrderBy(x => x.User!.FullName).Select(x => new { id = x.Id, value = x.Id, label = x.User!.FullName, name = x.User!.FullName }).ToListAsync();
    var courses = await db.Courses.OrderBy(x => x.Title).Select(x => new { id = x.Id, value = x.Id, label = x.Title, title = x.Title, name = x.Title }).ToListAsync();
    var users = await db.Users.OrderBy(x => x.FullName).Select(x => new { id = x.Id, value = x.Id, label = x.FullName, name = x.FullName }).ToListAsync();
    return Results.Ok(new { establishments, departments, filieres, professors, courses, users });
});

// Establishments
api.MapGet("/establishments", async (ApplicationDbContext db) =>
    Results.Ok(await db.Establishments.OrderBy(x => x.Name).ToListAsync()));

api.MapPost("/establishments", async (ApplicationDbContext db, Establishment model) =>
{
    db.Establishments.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/establishments/{id:int}", async (ApplicationDbContext db, int id, Establishment model) =>
{
    var item = await db.Establishments.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Name = model.Name;
    item.Code = model.Code;
    item.City = model.City;
    item.Address = model.Address;
    item.Phone = model.Phone;
    item.Email = model.Email;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/establishments/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Establishments.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Establishments.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Departments
api.MapGet("/departments", async (ApplicationDbContext db) =>
    Results.Ok(await db.Departments.Include(x => x.Establishment)
        .OrderBy(x => x.Name)
        .Select(x => new
        {
            x.Id, x.Name, x.Code, x.HeadName, x.EstablishmentId,
            establishmentName = x.Establishment!.Name
        }).ToListAsync()));

api.MapPost("/departments", async (ApplicationDbContext db, Department model) =>
{
    db.Departments.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/departments/{id:int}", async (ApplicationDbContext db, int id, Department model) =>
{
    var item = await db.Departments.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Name = model.Name;
    item.Code = model.Code;
    item.HeadName = model.HeadName;
    item.EstablishmentId = model.EstablishmentId;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/departments/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Departments.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Departments.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Filieres
api.MapGet("/filieres", async (ApplicationDbContext db) =>
    Results.Ok(await db.Filieres.Include(x => x.Department)
        .OrderBy(x => x.Name)
        .Select(x => new { x.Id, x.Name, x.Code, x.Cycle, x.DepartmentId, departmentName = x.Department!.Name }).ToListAsync()));

api.MapPost("/filieres", async (ApplicationDbContext db, Filiere model) =>
{
    db.Filieres.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/filieres/{id:int}", async (ApplicationDbContext db, int id, Filiere model) =>
{
    var item = await db.Filieres.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Name = model.Name;
    item.Code = model.Code;
    item.Cycle = model.Cycle;
    item.DepartmentId = model.DepartmentId;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/filieres/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Filieres.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Filieres.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Students
api.MapGet("/students", async (ApplicationDbContext db) =>
    Results.Ok(await db.Students.Include(x => x.User).Include(x => x.Filiere)
        .OrderBy(x => x.User!.FullName)
        .Select(x => new
        {
            x.Id,
            fullName = x.User!.FullName,
            email = x.User!.Email,
            avatar = x.User!.Avatar,
            x.StudentNumber,
            cne = x.Cne,
            x.Gender,
            x.Level,
            x.Status,
            x.FiliereId,
            filiereName = x.Filiere!.Name,
            enrollmentDate = x.EnrollmentDate.ToString("yyyy-MM-dd")
        }).ToListAsync()));

api.MapPost("/students", async (ApplicationDbContext db, StudentRequest req) =>
{
    var user = new AppUser
    {
        FullName = req.FullName,
        Email = req.Email,
        PasswordHash = string.IsNullOrWhiteSpace(req.Password) ? "123456" : req.Password,
        Avatar = string.IsNullOrWhiteSpace(req.Avatar) ? GetAvatar(req.FullName) : req.Avatar,
        Role = Roles.Student,
        IsActive = true
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();

    var student = new StudentProfile
    {
        UserId = user.Id,
        StudentNumber = req.StudentNumber,
        Cne = req.Cne,
        Gender = req.Gender ?? "",
        Level = req.Level,
        Status = req.Status,
        FiliereId = req.FiliereId,
        EnrollmentDate = req.EnrollmentDate ?? DateTime.UtcNow
    };
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Ok(new { student.Id });
});

api.MapPut("/students/{id:int}", async (ApplicationDbContext db, int id, StudentRequest req) =>
{
    var item = await db.Students.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
    if (item is null || item.User is null) return Results.NotFound();
    item.User.FullName = req.FullName;
    item.User.Email = req.Email;
    item.User.Avatar = string.IsNullOrWhiteSpace(req.Avatar) ? GetAvatar(req.FullName) : req.Avatar;
    if (!string.IsNullOrWhiteSpace(req.Password)) item.User.PasswordHash = req.Password;
    item.StudentNumber = req.StudentNumber;
    item.Cne = req.Cne;
    item.Gender = req.Gender ?? "";
    item.Level = req.Level;
    item.Status = req.Status;
    item.FiliereId = req.FiliereId;
    item.EnrollmentDate = req.EnrollmentDate ?? item.EnrollmentDate;
    await db.SaveChangesAsync();
    return Results.Ok(new { item.Id });
});

api.MapDelete("/students/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Students.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
    if (item is null || item.User is null) return Results.NotFound();
    db.Users.Remove(item.User);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Professors
api.MapGet("/professors", async (ApplicationDbContext db) =>
    Results.Ok(await db.Professors.Include(x => x.User).Include(x => x.Department)
        .OrderBy(x => x.User!.FullName)
        .Select(x => new
        {
            x.Id,
            fullName = x.User!.FullName,
            email = x.User!.Email,
            avatar = x.User!.Avatar,
            x.DepartmentId,
            departmentName = x.Department!.Name,
            x.Grade,
            x.Speciality,
            x.Office,
            x.Status
        }).ToListAsync()));

api.MapPost("/professors", async (ApplicationDbContext db, ProfessorRequest req) =>
{
    var user = new AppUser
    {
        FullName = req.FullName,
        Email = req.Email,
        PasswordHash = string.IsNullOrWhiteSpace(req.Password) ? "123456" : req.Password,
        Avatar = string.IsNullOrWhiteSpace(req.Avatar) ? GetAvatar(req.FullName) : req.Avatar,
        Role = Roles.Professor,
        IsActive = true
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();

    var professor = new ProfessorProfile
    {
        UserId = user.Id,
        DepartmentId = req.DepartmentId,
        Grade = req.Grade,
        Speciality = req.Speciality,
        Office = req.Office ?? "",
        Status = req.Status
    };
    db.Professors.Add(professor);
    await db.SaveChangesAsync();
    return Results.Ok(new { professor.Id });
});

api.MapPut("/professors/{id:int}", async (ApplicationDbContext db, int id, ProfessorRequest req) =>
{
    var item = await db.Professors.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
    if (item is null || item.User is null) return Results.NotFound();
    item.User.FullName = req.FullName;
    item.User.Email = req.Email;
    item.User.Avatar = string.IsNullOrWhiteSpace(req.Avatar) ? GetAvatar(req.FullName) : req.Avatar;
    if (!string.IsNullOrWhiteSpace(req.Password)) item.User.PasswordHash = req.Password;
    item.DepartmentId = req.DepartmentId;
    item.Grade = req.Grade;
    item.Speciality = req.Speciality;
    item.Office = req.Office ?? "";
    item.Status = req.Status;
    await db.SaveChangesAsync();
    return Results.Ok(new { item.Id });
});

api.MapDelete("/professors/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Professors.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
    if (item is null || item.User is null) return Results.NotFound();
    db.Users.Remove(item.User);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Laboratories
api.MapGet("/laboratories", async (ApplicationDbContext db) =>
    Results.Ok(await db.Laboratories.Include(x => x.Department).Include(x => x.ResponsibleProfessor)!.ThenInclude(x => x!.User)
        .OrderBy(x => x.Name)
        .Select(x => new
        {
            x.Id, x.Name, x.Code, x.Location, x.MainEquipment, x.Capacity, x.DepartmentId,
            departmentName = x.Department!.Name,
            x.ResponsibleProfessorId,
            responsibleProfessorName = x.ResponsibleProfessor != null ? x.ResponsibleProfessor.User!.FullName : "—"
        }).ToListAsync()));

api.MapPost("/laboratories", async (ApplicationDbContext db, Laboratory model) =>
{
    db.Laboratories.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/laboratories/{id:int}", async (ApplicationDbContext db, int id, Laboratory model) =>
{
    var item = await db.Laboratories.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Name = model.Name; item.Code = model.Code; item.Location = model.Location; item.MainEquipment = model.MainEquipment;
    item.Capacity = model.Capacity; item.DepartmentId = model.DepartmentId; item.ResponsibleProfessorId = model.ResponsibleProfessorId;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/laboratories/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Laboratories.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Laboratories.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Courses
api.MapGet("/courses", async (ApplicationDbContext db) =>
    Results.Ok(await db.Courses.Include(x => x.Filiere).Include(x => x.Professor)!.ThenInclude(x => x!.User)
        .OrderBy(x => x.Title)
        .Select(x => new
        {
            x.Id, x.Code, title = x.Title, x.Description, x.Credits, x.Semester, x.IsOnline,
            x.FiliereId, filiereName = x.Filiere!.Name,
            x.ProfessorId, professorName = x.Professor!.User!.FullName
        }).ToListAsync()));

api.MapPost("/courses", async (ApplicationDbContext db, Course model) =>
{
    db.Courses.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/courses/{id:int}", async (ApplicationDbContext db, int id, Course model) =>
{
    var item = await db.Courses.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Code = model.Code; item.Title = model.Title; item.Description = model.Description; item.Credits = model.Credits;
    item.Semester = model.Semester; item.IsOnline = model.IsOnline; item.FiliereId = model.FiliereId; item.ProfessorId = model.ProfessorId;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/courses/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Courses.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Courses.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Timetable
api.MapGet("/timetable", async (ApplicationDbContext db) =>
    Results.Ok(await db.TimetableEntries.Include(x => x.Course).Include(x => x.Professor)!.ThenInclude(x => x!.User).Include(x => x.Filiere)
        .OrderBy(x => x.Day).ThenBy(x => x.TimeSlot)
        .Select(x => new
        {
            x.Id, x.CourseId, courseTitle = x.Course!.Title, x.ProfessorId, professorName = x.Professor!.User!.FullName,
            x.FiliereId, filiereName = x.Filiere!.Name, x.Day, x.TimeSlot, x.Room, x.GroupName, x.SessionType
        }).ToListAsync()));

api.MapPost("/timetable", async (ApplicationDbContext db, TimetableEntry model) =>
{
    db.TimetableEntries.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/timetable/{id:int}", async (ApplicationDbContext db, int id, TimetableEntry model) =>
{
    var item = await db.TimetableEntries.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.CourseId = model.CourseId; item.ProfessorId = model.ProfessorId; item.FiliereId = model.FiliereId; item.Day = model.Day;
    item.TimeSlot = model.TimeSlot; item.Room = model.Room; item.GroupName = model.GroupName; item.SessionType = model.SessionType;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/timetable/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.TimetableEntries.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.TimetableEntries.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Resources
api.MapGet("/resources", async (ApplicationDbContext db) =>
    Results.Ok(await db.CourseResources.Include(x => x.Course)
        .OrderByDescending(x => x.PublishedAt)
        .Select(x => new { x.Id, x.CourseId, courseTitle = x.Course!.Title, x.Title, x.Type, x.Url, x.Published }).ToListAsync()));

api.MapPost("/resources", async (ApplicationDbContext db, CourseResource model) =>
{
    db.CourseResources.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/resources/{id:int}", async (ApplicationDbContext db, int id, CourseResource model) =>
{
    var item = await db.CourseResources.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.CourseId = model.CourseId; item.Title = model.Title; item.Type = model.Type; item.Url = model.Url; item.Published = model.Published;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/resources/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.CourseResources.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.CourseResources.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Assignments
api.MapGet("/assignments", async (ApplicationDbContext db) =>
    Results.Ok(await db.Assignments.Include(x => x.Course)
        .OrderBy(x => x.DueDate)
        .Select(x => new { x.Id, x.CourseId, courseTitle = x.Course!.Title, x.Title, x.Description, dueDate = x.DueDate.ToString("yyyy-MM-ddTHH:mm"), x.TotalPoints, x.Status }).ToListAsync()));

api.MapPost("/assignments", async (ApplicationDbContext db, Assignment model) =>
{
    db.Assignments.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/assignments/{id:int}", async (ApplicationDbContext db, int id, Assignment model) =>
{
    var item = await db.Assignments.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.CourseId = model.CourseId; item.Title = model.Title; item.Description = model.Description; item.DueDate = model.DueDate; item.TotalPoints = model.TotalPoints; item.Status = model.Status;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/assignments/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Assignments.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Assignments.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Quizzes
api.MapGet("/quizzes", async (ApplicationDbContext db) =>
    Results.Ok(await db.Quizzes.Include(x => x.Course)
        .OrderByDescending(x => x.StartAt)
        .Select(x => new { x.Id, x.CourseId, courseTitle = x.Course!.Title, x.Title, x.Questions, x.DurationMinutes, x.Availability, startAt = x.StartAt.ToString("yyyy-MM-ddTHH:mm"), endAt = x.EndAt.ToString("yyyy-MM-ddTHH:mm") }).ToListAsync()));

api.MapPost("/quizzes", async (ApplicationDbContext db, Quiz model) =>
{
    db.Quizzes.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/quizzes/{id:int}", async (ApplicationDbContext db, int id, Quiz model) =>
{
    var item = await db.Quizzes.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.CourseId = model.CourseId; item.Title = model.Title; item.Questions = model.Questions; item.DurationMinutes = model.DurationMinutes; item.Availability = model.Availability; item.StartAt = model.StartAt; item.EndAt = model.EndAt;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/quizzes/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Quizzes.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Quizzes.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Forums
api.MapGet("/forums", async (ApplicationDbContext db) =>
    Results.Ok(await db.ForumTopics.Include(x => x.CreatedByUser).Include(x => x.Course)
        .OrderByDescending(x => x.CreatedAt)
        .Select(x => new { x.Id, x.Title, x.Forum, x.CreatedByUserId, createdByName = x.CreatedByUser!.FullName, x.CourseId, courseTitle = x.Course != null ? x.Course.Title : "—", x.IsClosed }).ToListAsync()));

api.MapPost("/forums", async (ApplicationDbContext db, ForumTopic model) =>
{
    db.ForumTopics.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok(model);
});

api.MapPut("/forums/{id:int}", async (ApplicationDbContext db, int id, ForumTopic model) =>
{
    var item = await db.ForumTopics.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Title = model.Title; item.Forum = model.Forum; item.CreatedByUserId = model.CreatedByUserId; item.CourseId = model.CourseId; item.IsClosed = model.IsClosed;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

api.MapDelete("/forums/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.ForumTopics.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.ForumTopics.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Notifications
api.MapGet("/notifications", async (ApplicationDbContext db) =>
    Results.Ok(await db.Notifications.Include(x => x.User).OrderByDescending(x => x.CreatedAt)
        .Select(x => new { x.Id, x.Title, x.Message, x.Type, x.IsRead, createdAt = x.CreatedAt.ToString("yyyy-MM-dd HH:mm"), userName = x.User!.FullName }).ToListAsync()));

api.MapDelete("/notifications/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var item = await db.Notifications.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Notifications.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

static string GetAvatar(string fullName)
{
    var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length == 0) return "UM";
    if (parts.Length == 1) return parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();
    return string.Concat(parts[0][0], parts[^1][0]).ToUpperInvariant();
}

record StudentRequest(
    string FullName,
    string Email,
    string? Password,
    string? Avatar,
    string StudentNumber,
    string Cne,
    string? Gender,
    string Level,
    string Status,
    int FiliereId,
    DateTime? EnrollmentDate
);

record ProfessorRequest(
    string FullName,
    string Email,
    string? Password,
    string? Avatar,
    int DepartmentId,
    string Grade,
    string Speciality,
    string? Office,
    string Status
);
