using Microsoft.EntityFrameworkCore;
using UniversityManagement.Api.Models;

namespace UniversityManagement.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Users.AnyAsync())
        {
            return;
        }

        var fds = new Establishment
        {
            Name = "Faculté des Sciences",
            Code = "FDS",
            City = "Tanger",
            Address = "Route de Rabat",
            Phone = "0539-000111",
            Email = "contact-fds@university.ma"
        };

        var ensa = new Establishment
        {
            Name = "ENSA Tanger",
            Code = "ENSA",
            City = "Tanger",
            Address = "Mghogha",
            Phone = "0539-000222",
            Email = "contact-ensa@university.ma"
        };

        db.Establishments.AddRange(fds, ensa);
        await db.SaveChangesAsync();

        var depInfo = new Department { Name = "Informatique", Code = "INFO", EstablishmentId = ensa.Id, HeadName = "Pr. Nadia Chafai" };
        var depMath = new Department { Name = "Mathématiques", Code = "MATH", EstablishmentId = fds.Id, HeadName = "Pr. Jalal Bennis" };
        var depRsi = new Department { Name = "Réseaux & Systèmes", Code = "RSI", EstablishmentId = ensa.Id, HeadName = "Pr. Karim El Idrissi" };
        db.Departments.AddRange(depInfo, depMath, depRsi);
        await db.SaveChangesAsync();

        var gi = new Filiere { Name = "Génie Informatique", Code = "GI", Cycle = "Ingénieur", DepartmentId = depInfo.Id };
        var ds = new Filiere { Name = "Data Science", Code = "DS", Cycle = "Master", DepartmentId = depInfo.Id };
        var ma = new Filiere { Name = "Mathématiques Appliquées", Code = "MA", Cycle = "Licence", DepartmentId = depMath.Id };
        db.Filieres.AddRange(gi, ds, ma);

        var currentYear = new AcademicYear
        {
            Label = "2025-2026",
            StartDate = new DateTime(2025, 9, 1),
            EndDate = new DateTime(2026, 7, 31),
            IsCurrent = true
        };
        db.AcademicYears.Add(currentYear);
        await db.SaveChangesAsync();

        var adminUser = new AppUser
        {
            FullName = "Aya El Harak",
            Email = "admin@university.ma",
            PasswordHash = "123456",
            Role = Roles.Administration,
            Avatar = "AE"
        };
        var profUser = new AppUser
        {
            FullName = "Pr. Nadia Chafai",
            Email = "prof@university.ma",
            PasswordHash = "123456",
            Role = Roles.Professor,
            Avatar = "NC"
        };
        var studentUser = new AppUser
        {
            FullName = "Youssef Amrani",
            Email = "student@university.ma",
            PasswordHash = "123456",
            Role = Roles.Student,
            Avatar = "YA"
        };
        var profUser2 = new AppUser
        {
            FullName = "Pr. Karim El Idrissi",
            Email = "karim@prof.ma",
            PasswordHash = "123456",
            Role = Roles.Professor,
            Avatar = "KE"
        };
        var profUser3 = new AppUser
        {
            FullName = "Pr. Jalal Bennis",
            Email = "jalal@prof.ma",
            PasswordHash = "123456",
            Role = Roles.Professor,
            Avatar = "JB"
        };
        var studentUser2 = new AppUser
        {
            FullName = "Sara El Ghazi",
            Email = "sara@etu.ma",
            PasswordHash = "123456",
            Role = Roles.Student,
            Avatar = "SG"
        };
        var studentUser3 = new AppUser
        {
            FullName = "Mehdi Boulahya",
            Email = "mehdi@etu.ma",
            PasswordHash = "123456",
            Role = Roles.Student,
            Avatar = "MB"
        };
        db.Users.AddRange(adminUser, profUser, studentUser, profUser2, profUser3, studentUser2, studentUser3);
        await db.SaveChangesAsync();

        var professor1 = new ProfessorProfile
        {
            UserId = profUser.Id,
            DepartmentId = depInfo.Id,
            Grade = "Professeur Habilité",
            Speciality = "IA & Développement Web",
            Office = "B-212"
        };
        var professor2 = new ProfessorProfile
        {
            UserId = profUser2.Id,
            DepartmentId = depRsi.Id,
            Grade = "Professeur",
            Speciality = "Cloud & Sécurité",
            Office = "LAB-Admin"
        };
        var professor3 = new ProfessorProfile
        {
            UserId = profUser3.Id,
            DepartmentId = depMath.Id,
            Grade = "Professeur Assistant",
            Speciality = "Optimisation",
            Office = "M-104"
        };
        db.Professors.AddRange(professor1, professor2, professor3);
        await db.SaveChangesAsync();

        var student1 = new StudentProfile
        {
            UserId = studentUser.Id,
            StudentNumber = "ETU-2026-001",
            Cne = "CNE001",
            Gender = "Homme",
            Level = "3ème année",
            Status = "Actif",
            FiliereId = gi.Id,
            EnrollmentDate = new DateTime(2025, 9, 5)
        };
        var student2 = new StudentProfile
        {
            UserId = studentUser2.Id,
            StudentNumber = "ETU-2026-002",
            Cne = "CNE002",
            Gender = "Femme",
            Level = "2ème année",
            Status = "Actif",
            FiliereId = gi.Id,
            EnrollmentDate = new DateTime(2025, 9, 8)
        };
        var student3 = new StudentProfile
        {
            UserId = studentUser3.Id,
            StudentNumber = "ETU-2026-003",
            Cne = "CNE003",
            Gender = "Homme",
            Level = "Master 1",
            Status = "Actif",
            FiliereId = ds.Id,
            EnrollmentDate = new DateTime(2025, 9, 9)
        };
        db.Students.AddRange(student1, student2, student3);
        await db.SaveChangesAsync();

        db.Enrollments.AddRange(
            new Enrollment { StudentId = student1.Id, FiliereId = gi.Id, AcademicYearId = currentYear.Id, GroupName = "GI3-A" },
            new Enrollment { StudentId = student2.Id, FiliereId = gi.Id, AcademicYearId = currentYear.Id, GroupName = "GI2-A" },
            new Enrollment { StudentId = student3.Id, FiliereId = ds.Id, AcademicYearId = currentYear.Id, GroupName = "DS-M1" }
        );

        var lab1 = new Laboratory
        {
            Name = "Laboratoire IA & Data",
            Code = "LAB-IA",
            Location = "Bloc C - Salle 305",
            MainEquipment = "GPU, Stations IA, Serveur ML",
            Capacity = 24,
            DepartmentId = depInfo.Id,
            ResponsibleProfessorId = professor1.Id
        };
        var lab2 = new Laboratory
        {
            Name = "Laboratoire Réseaux",
            Code = "LAB-NET",
            Location = "Bloc A - Salle 102",
            MainEquipment = "Switchs, Routeurs, Firewall, Baie",
            Capacity = 18,
            DepartmentId = depRsi.Id,
            ResponsibleProfessorId = professor2.Id
        };
        db.Laboratories.AddRange(lab1, lab2);
        await db.SaveChangesAsync();

        var course1 = new Course
        {
            Code = "WEB301",
            Title = "Développement Web Avancé",
            Description = "React, API REST et architecture front moderne.",
            Credits = 6,
            Semester = "S5",
            FiliereId = gi.Id,
            ProfessorId = professor1.Id,
            IsOnline = true
        };
        var course2 = new Course
        {
            Code = "NET401",
            Title = "Administration Réseau",
            Description = "Réseaux, VLAN, sécurité, supervision.",
            Credits = 5,
            Semester = "S5",
            FiliereId = gi.Id,
            ProfessorId = professor2.Id,
            IsOnline = true
        };
        var course3 = new Course
        {
            Code = "DS510",
            Title = "Machine Learning",
            Description = "Régression, classification et notebooks.",
            Credits = 6,
            Semester = "S1",
            FiliereId = ds.Id,
            ProfessorId = professor1.Id,
            IsOnline = true
        };
        var course4 = new Course
        {
            Code = "MAT210",
            Title = "Algèbre Linéaire",
            Description = "Matrices, espaces vectoriels et applications.",
            Credits = 4,
            Semester = "S3",
            FiliereId = ma.Id,
            ProfessorId = professor3.Id,
            IsOnline = false
        };
        db.Courses.AddRange(course1, course2, course3, course4);
        await db.SaveChangesAsync();

        db.CourseResources.AddRange(
            new CourseResource { CourseId = course1.Id, Title = "Chapitre 1 - React Fundamentals", Type = "Vidéo", Url = "https://example.com/react", Published = true },
            new CourseResource { CourseId = course3.Id, Title = "TP 1 - Régression linéaire", Type = "PDF", Url = "https://example.com/ml-regression", Published = true },
            new CourseResource { CourseId = course2.Id, Title = "Guide VLAN & switching", Type = "Document", Url = "https://example.com/vlan-guide", Published = true }
        );

        db.TimetableEntries.AddRange(
            new TimetableEntry { CourseId = course1.Id, ProfessorId = professor1.Id, FiliereId = gi.Id, Day = "Lundi", TimeSlot = "09:00 - 11:00", Room = "A-201", GroupName = "GI3-A", SessionType = "Cours" },
            new TimetableEntry { CourseId = course2.Id, ProfessorId = professor2.Id, FiliereId = gi.Id, Day = "Mardi", TimeSlot = "14:00 - 16:00", Room = "LAB-NET", GroupName = "GI3-A", SessionType = "TP" },
            new TimetableEntry { CourseId = course3.Id, ProfessorId = professor1.Id, FiliereId = ds.Id, Day = "Jeudi", TimeSlot = "10:00 - 12:00", Room = "B-104", GroupName = "DS-M1", SessionType = "Cours" }
        );

        var assign1 = new Assignment
        {
            CourseId = course1.Id,
            Title = "Mini projet React",
            Description = "Créer une interface responsive connectée à une API.",
            DueDate = new DateTime(2026, 3, 24),
            TotalPoints = 20,
            Status = "Ouvert"
        };
        var assign2 = new Assignment
        {
            CourseId = course3.Id,
            Title = "Notebook de classification",
            Description = "Comparer plusieurs algorithmes de classification.",
            DueDate = new DateTime(2026, 3, 27),
            TotalPoints = 20,
            Status = "Ouvert"
        };
        db.Assignments.AddRange(assign1, assign2);
        await db.SaveChangesAsync();

        db.AssignmentSubmissions.Add(
            new AssignmentSubmission
            {
                AssignmentId = assign1.Id,
                StudentId = student2.Id,
                FileUrl = "https://example.com/submission-react",
                Grade = 17,
                Feedback = "Bon travail, améliorer la validation.",
                Status = "Corrigé"
            }
        );

        var quiz1 = new Quiz
        {
            CourseId = course1.Id,
            Title = "Quiz JSX & Components",
            Questions = 12,
            DurationMinutes = 20,
            Availability = "Disponible",
            StartAt = new DateTime(2026, 3, 18, 8, 0, 0),
            EndAt = new DateTime(2026, 3, 20, 23, 0, 0)
        };
        var quiz2 = new Quiz
        {
            CourseId = course3.Id,
            Title = "Quiz Régression",
            Questions = 15,
            DurationMinutes = 25,
            Availability = "Planifié",
            StartAt = new DateTime(2026, 3, 21, 8, 0, 0),
            EndAt = new DateTime(2026, 3, 23, 23, 0, 0)
        };
        db.Quizzes.AddRange(quiz1, quiz2);
        await db.SaveChangesAsync();

        db.QuizQuestions.AddRange(
            new QuizQuestion { QuizId = quiz1.Id, QuestionText = "À quoi sert un composant React ?", CorrectAnswer = "Réutiliser l'UI", Marks = 2 },
            new QuizQuestion { QuizId = quiz1.Id, QuestionText = "Que retourne JSX après transpilation ?", CorrectAnswer = "createElement", Marks = 2 },
            new QuizQuestion { QuizId = quiz2.Id, QuestionText = "Quel algorithme pour prédire une valeur continue ?", CorrectAnswer = "Régression linéaire", Marks = 2 }
        );

        var topic1 = new ForumTopic
        {
            Forum = "Forum général",
            Title = "Bienvenue sur la plateforme universitaire",
            CreatedByUserId = adminUser.Id,
            CreatedAt = DateTime.UtcNow.AddHours(-3)
        };
        var topic2 = new ForumTopic
        {
            Forum = "Développement Web Avancé",
            Title = "Questions sur le mini projet React",
            CreatedByUserId = profUser.Id,
            CourseId = course1.Id,
            CreatedAt = DateTime.UtcNow.AddHours(-2)
        };
        db.ForumTopics.AddRange(topic1, topic2);
        await db.SaveChangesAsync();

        db.ForumMessages.AddRange(
            new ForumMessage { TopicId = topic1.Id, CreatedByUserId = adminUser.Id, Content = "Bienvenue à tous sur le portail.", CreatedAt = DateTime.UtcNow.AddHours(-3) },
            new ForumMessage { TopicId = topic2.Id, CreatedByUserId = profUser.Id, Content = "Posez ici vos questions sur le projet.", CreatedAt = DateTime.UtcNow.AddHours(-2) },
            new ForumMessage { TopicId = topic2.Id, CreatedByUserId = studentUser.Id, Content = "Peut-on utiliser Bootstrap avec React ?", CreatedAt = DateTime.UtcNow.AddHours(-1) }
        );

        db.AttendanceRecords.AddRange(
            new AttendanceRecord { StudentId = student1.Id, CourseId = course1.Id, SessionDate = new DateTime(2026, 3, 10), Status = "Présent" },
            new AttendanceRecord { StudentId = student1.Id, CourseId = course2.Id, SessionDate = new DateTime(2026, 3, 11), Status = "Absent" },
            new AttendanceRecord { StudentId = student1.Id, CourseId = course1.Id, SessionDate = new DateTime(2026, 3, 17), Status = "Présent" }
        );

        db.GradeRecords.AddRange(
            new GradeRecord { StudentId = student1.Id, CourseId = course1.Id, Label = "CC1", Score = 15.50m, MaxScore = 20m, RecordedAt = DateTime.UtcNow.AddDays(-10) },
            new GradeRecord { StudentId = student1.Id, CourseId = course2.Id, Label = "TP Réseaux", Score = 14.00m, MaxScore = 20m, RecordedAt = DateTime.UtcNow.AddDays(-7) },
            new GradeRecord { StudentId = student2.Id, CourseId = course1.Id, Label = "CC1", Score = 17.00m, MaxScore = 20m, RecordedAt = DateTime.UtcNow.AddDays(-10) }
        );

        db.Notifications.AddRange(
            new NotificationItem { UserId = studentUser.Id, Title = "Nouveau devoir", Message = "Le mini projet React a été publié.", Type = "Assignment", IsRead = false },
            new NotificationItem { UserId = studentUser.Id, Title = "Mise à jour emploi du temps", Message = "Le créneau du jeudi a été déplacé.", Type = "Timetable", IsRead = true },
            new NotificationItem { UserId = profUser.Id, Title = "Nouvelle question forum", Message = "Un étudiant a posé une question sur le projet.", Type = "Forum", IsRead = false },
            new NotificationItem { UserId = adminUser.Id, Title = "Réservation laboratoire", Message = "Une nouvelle demande de réservation a été validée.", Type = "Laboratory", IsRead = false }
        );

        db.LaboratoryReservations.Add(
            new LaboratoryReservation
            {
                LaboratoryId = lab1.Id,
                ReservedByUserId = studentUser.Id,
                ReservationDate = new DateTime(2026, 3, 19),
                TimeSlot = "15:00 - 17:00",
                Purpose = "Travail de groupe projet IA",
                Status = "Validée"
            }
        );

        await db.SaveChangesAsync();
    }
}
