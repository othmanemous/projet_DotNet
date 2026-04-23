-- University Management - MySQL / XAMPP
-- Version alignée avec les entités EF Core du projet

CREATE DATABASE IF NOT EXISTS university_management_db
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE university_management_db;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS Notifications;
DROP TABLE IF EXISTS GradeRecords;
DROP TABLE IF EXISTS AttendanceRecords;
DROP TABLE IF EXISTS ForumMessages;
DROP TABLE IF EXISTS ForumTopics;
DROP TABLE IF EXISTS QuizQuestions;
DROP TABLE IF EXISTS Quizzes;
DROP TABLE IF EXISTS AssignmentSubmissions;
DROP TABLE IF EXISTS Assignments;
DROP TABLE IF EXISTS TimetableEntries;
DROP TABLE IF EXISTS Enrollments;
DROP TABLE IF EXISTS CourseResources;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS LaboratoryReservations;
DROP TABLE IF EXISTS Laboratories;
DROP TABLE IF EXISTS Students;
DROP TABLE IF EXISTS Professors;
DROP TABLE IF EXISTS AcademicYears;
DROP TABLE IF EXISTS Filieres;
DROP TABLE IF EXISTS Departments;
DROP TABLE IF EXISTS Establishments;
DROP TABLE IF EXISTS Users;

SET FOREIGN_KEY_CHECKS = 1;

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(150) NOT NULL,
    Role VARCHAR(40) NOT NULL,
    Avatar VARCHAR(10) NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Establishments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(180) NOT NULL,
    Code VARCHAR(20) NOT NULL UNIQUE,
    City VARCHAR(120) NOT NULL,
    Address VARCHAR(220) NOT NULL,
    Phone VARCHAR(40) NOT NULL,
    Email VARCHAR(150) NOT NULL
);

CREATE TABLE Departments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EstablishmentId INT NOT NULL,
    Name VARCHAR(150) NOT NULL,
    Code VARCHAR(20) NOT NULL UNIQUE,
    HeadName VARCHAR(150) NOT NULL,
    CONSTRAINT FK_Departments_Establishments
        FOREIGN KEY (EstablishmentId) REFERENCES Establishments(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Filieres (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DepartmentId INT NOT NULL,
    Name VARCHAR(150) NOT NULL,
    Code VARCHAR(20) NOT NULL UNIQUE,
    Cycle VARCHAR(80) NOT NULL,
    CONSTRAINT FK_Filieres_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE AcademicYears (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Label VARCHAR(20) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsCurrent TINYINT(1) NOT NULL DEFAULT 0
);

CREATE TABLE Professors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    DepartmentId INT NOT NULL,
    Grade VARCHAR(100) NOT NULL,
    Speciality VARCHAR(150) NOT NULL,
    Office VARCHAR(40) NOT NULL,
    Status VARCHAR(30) NOT NULL DEFAULT 'Actif',
    CONSTRAINT FK_Professors_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Professors_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE Students (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    StudentNumber VARCHAR(40) NOT NULL UNIQUE,
    Cne VARCHAR(40) NOT NULL UNIQUE,
    Gender VARCHAR(20) NOT NULL,
    Level VARCHAR(80) NOT NULL,
    Status VARCHAR(30) NOT NULL DEFAULT 'Actif',
    FiliereId INT NOT NULL,
    EnrollmentDate DATE NOT NULL,
    CONSTRAINT FK_Students_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Students_Filieres
        FOREIGN KEY (FiliereId) REFERENCES Filieres(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE Laboratories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DepartmentId INT NOT NULL,
    ResponsibleProfessorId INT NULL,
    Name VARCHAR(160) NOT NULL,
    Code VARCHAR(20) NOT NULL UNIQUE,
    Location VARCHAR(100) NOT NULL,
    MainEquipment VARCHAR(250) NOT NULL,
    Capacity INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Laboratories_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Laboratories_Professors
        FOREIGN KEY (ResponsibleProfessorId) REFERENCES Professors(Id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE LaboratoryReservations (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    LaboratoryId INT NOT NULL,
    ReservedByUserId INT NOT NULL,
    ReservationDate DATE NOT NULL,
    TimeSlot VARCHAR(80) NOT NULL,
    Purpose VARCHAR(200) NOT NULL,
    Status VARCHAR(30) NOT NULL DEFAULT 'Validée',
    CONSTRAINT FK_LaboratoryReservations_Laboratories
        FOREIGN KEY (LaboratoryId) REFERENCES Laboratories(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_LaboratoryReservations_Users
        FOREIGN KEY (ReservedByUserId) REFERENCES Users(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Courses (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FiliereId INT NOT NULL,
    ProfessorId INT NOT NULL,
    Code VARCHAR(30) NOT NULL UNIQUE,
    Title VARCHAR(180) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    Credits INT NOT NULL DEFAULT 0,
    Semester VARCHAR(20) NOT NULL,
    IsOnline TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT FK_Courses_Filieres
        FOREIGN KEY (FiliereId) REFERENCES Filieres(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT FK_Courses_Professors
        FOREIGN KEY (ProfessorId) REFERENCES Professors(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE CourseResources (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CourseId INT NOT NULL,
    Title VARCHAR(180) NOT NULL,
    Type VARCHAR(30) NOT NULL,
    Url VARCHAR(250) NOT NULL,
    Published TINYINT(1) NOT NULL DEFAULT 1,
    PublishedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_CourseResources_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Enrollments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    FiliereId INT NOT NULL,
    AcademicYearId INT NOT NULL,
    GroupName VARCHAR(30) NOT NULL,
    Status VARCHAR(30) NOT NULL DEFAULT 'Inscrit',
    CONSTRAINT FK_Enrollments_Students
        FOREIGN KEY (StudentId) REFERENCES Students(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Enrollments_Filieres
        FOREIGN KEY (FiliereId) REFERENCES Filieres(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT FK_Enrollments_AcademicYears
        FOREIGN KEY (AcademicYearId) REFERENCES AcademicYears(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE TimetableEntries (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CourseId INT NOT NULL,
    ProfessorId INT NOT NULL,
    FiliereId INT NOT NULL,
    Day VARCHAR(20) NOT NULL,
    TimeSlot VARCHAR(30) NOT NULL,
    Room VARCHAR(60) NOT NULL,
    GroupName VARCHAR(30) NOT NULL,
    SessionType VARCHAR(30) NOT NULL DEFAULT 'Cours',
    CONSTRAINT FK_TimetableEntries_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_TimetableEntries_Professors
        FOREIGN KEY (ProfessorId) REFERENCES Professors(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT FK_TimetableEntries_Filieres
        FOREIGN KEY (FiliereId) REFERENCES Filieres(Id)
        ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE Assignments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CourseId INT NOT NULL,
    Title VARCHAR(180) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    DueDate DATETIME NOT NULL,
    TotalPoints INT NOT NULL DEFAULT 20,
    Status VARCHAR(30) NOT NULL DEFAULT 'Ouvert',
    CONSTRAINT FK_Assignments_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE AssignmentSubmissions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    AssignmentId INT NOT NULL,
    StudentId INT NOT NULL,
    SubmittedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FileUrl VARCHAR(250) NOT NULL,
    Grade INT NULL,
    Feedback VARCHAR(250) NULL,
    Status VARCHAR(30) NOT NULL DEFAULT 'Soumis',
    CONSTRAINT FK_AssignmentSubmissions_Assignments
        FOREIGN KEY (AssignmentId) REFERENCES Assignments(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_AssignmentSubmissions_Students
        FOREIGN KEY (StudentId) REFERENCES Students(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Quizzes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CourseId INT NOT NULL,
    Title VARCHAR(180) NOT NULL,
    Questions INT NOT NULL,
    DurationMinutes INT NOT NULL,
    Availability VARCHAR(30) NOT NULL DEFAULT 'Disponible',
    StartAt DATETIME NOT NULL,
    EndAt DATETIME NOT NULL,
    CONSTRAINT FK_Quizzes_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE QuizQuestions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    QuizId INT NOT NULL,
    QuestionText VARCHAR(500) NOT NULL,
    CorrectAnswer VARCHAR(250) NOT NULL,
    Marks INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_QuizQuestions_Quizzes
        FOREIGN KEY (QuizId) REFERENCES Quizzes(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE ForumTopics (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CreatedByUserId INT NOT NULL,
    CourseId INT NULL,
    Forum VARCHAR(100) NOT NULL DEFAULT 'Forum général',
    Title VARCHAR(180) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsClosed TINYINT(1) NOT NULL DEFAULT 0,
    CONSTRAINT FK_ForumTopics_Users
        FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_ForumTopics_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE ForumMessages (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TopicId INT NOT NULL,
    CreatedByUserId INT NOT NULL,
    Content VARCHAR(1000) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_ForumMessages_ForumTopics
        FOREIGN KEY (TopicId) REFERENCES ForumTopics(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_ForumMessages_Users
        FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE AttendanceRecords (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    SessionDate DATE NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Présent',
    CONSTRAINT FK_AttendanceRecords_Students
        FOREIGN KEY (StudentId) REFERENCES Students(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_AttendanceRecords_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE GradeRecords (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    Label VARCHAR(120) NOT NULL,
    Score DECIMAL(5,2) NOT NULL,
    MaxScore DECIMAL(5,2) NOT NULL DEFAULT 20.00,
    RecordedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_GradeRecords_Students
        FOREIGN KEY (StudentId) REFERENCES Students(Id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_GradeRecords_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Notifications (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Title VARCHAR(160) NOT NULL,
    Message VARCHAR(400) NOT NULL,
    Type VARCHAR(30) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsRead TINYINT(1) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Notifications_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

INSERT INTO Establishments (Name, Code, City, Address, Phone, Email) VALUES
('Faculté des Sciences', 'FDS', 'Tanger', 'Route de Rabat', '0539-000111', 'contact-fds@university.ma'),
('ENSA Tanger', 'ENSA', 'Tanger', 'Mghogha', '0539-000222', 'contact-ensa@university.ma');

INSERT INTO Departments (EstablishmentId, Name, Code, HeadName) VALUES
(2, 'Informatique', 'INFO', 'Pr. Nadia Chafai'),
(1, 'Mathématiques', 'MATH', 'Pr. Jalal Bennis'),
(2, 'Réseaux & Systèmes', 'RSI', 'Pr. Karim El Idrissi');

INSERT INTO Filieres (DepartmentId, Name, Code, Cycle) VALUES
(1, 'Génie Informatique', 'GI', 'Ingénieur'),
(1, 'Data Science', 'DS', 'Master'),
(2, 'Mathématiques Appliquées', 'MA', 'Licence');

INSERT INTO AcademicYears (Label, StartDate, EndDate, IsCurrent) VALUES
('2025-2026', '2025-09-01', '2026-07-31', 1);

INSERT INTO Users (FullName, Email, PasswordHash, Role, Avatar) VALUES
('Aya El Harak', 'admin@university.ma', '123456', 'Administration', 'AE'),
('Pr. Nadia Chafai', 'prof@university.ma', '123456', 'Professeur', 'NC'),
('Youssef Amrani', 'student@university.ma', '123456', 'Etudiant', 'YA'),
('Pr. Karim El Idrissi', 'karim@prof.ma', '123456', 'Professeur', 'KE'),
('Pr. Jalal Bennis', 'jalal@prof.ma', '123456', 'Professeur', 'JB'),
('Sara El Ghazi', 'sara@etu.ma', '123456', 'Etudiant', 'SG'),
('Mehdi Boulahya', 'mehdi@etu.ma', '123456', 'Etudiant', 'MB');

INSERT INTO Professors (UserId, DepartmentId, Grade, Speciality, Office, Status) VALUES
(2, 1, 'Professeur Habilité', 'IA & Développement Web', 'B-212', 'Actif'),
(4, 3, 'Professeur', 'Cloud & Sécurité', 'LAB-Admin', 'Actif'),
(5, 2, 'Professeur Assistant', 'Optimisation', 'M-104', 'Actif');

INSERT INTO Students (UserId, StudentNumber, Cne, Gender, Level, Status, FiliereId, EnrollmentDate) VALUES
(3, 'ETU-2026-001', 'CNE001', 'Homme', '3ème année', 'Actif', 1, '2025-09-05'),
(6, 'ETU-2026-002', 'CNE002', 'Femme', '2ème année', 'Actif', 1, '2025-09-08'),
(7, 'ETU-2026-003', 'CNE003', 'Homme', 'Master 1', 'Actif', 2, '2025-09-09');

INSERT INTO Laboratories (DepartmentId, ResponsibleProfessorId, Name, Code, Location, MainEquipment, Capacity) VALUES
(1, 1, 'Laboratoire IA & Data', 'LAB-IA', 'Bloc C - Salle 305', 'GPU, Stations IA, Serveur ML', 24),
(3, 2, 'Laboratoire Réseaux', 'LAB-NET', 'Bloc A - Salle 102', 'Switchs, Routeurs, Firewall, Baie', 18);

INSERT INTO Courses (FiliereId, ProfessorId, Code, Title, Description, Credits, Semester, IsOnline) VALUES
(1, 1, 'WEB301', 'Développement Web Avancé', 'React, API REST et architecture front moderne.', 6, 'S5', 1),
(1, 2, 'NET401', 'Administration Réseau', 'Réseaux, VLAN, sécurité, supervision.', 5, 'S5', 1),
(2, 1, 'DS510', 'Machine Learning', 'Régression, classification et notebooks.', 6, 'S1', 1),
(3, 3, 'MAT210', 'Algèbre Linéaire', 'Matrices, espaces vectoriels et applications.', 4, 'S3', 0);

INSERT INTO CourseResources (CourseId, Title, Type, Url, Published) VALUES
(1, 'Chapitre 1 - React Fundamentals', 'Vidéo', 'https://example.com/react', 1),
(3, 'TP 1 - Régression linéaire', 'PDF', 'https://example.com/ml-regression', 1),
(2, 'Guide VLAN & switching', 'Document', 'https://example.com/vlan-guide', 1);

INSERT INTO Enrollments (StudentId, FiliereId, AcademicYearId, GroupName, Status) VALUES
(1, 1, 1, 'GI3-A', 'Inscrit'),
(2, 1, 1, 'GI2-A', 'Inscrit'),
(3, 2, 1, 'DS-M1', 'Inscrit');

INSERT INTO TimetableEntries (CourseId, ProfessorId, FiliereId, Day, TimeSlot, Room, GroupName, SessionType) VALUES
(1, 1, 1, 'Lundi', '09:00 - 11:00', 'A-201', 'GI3-A', 'Cours'),
(2, 2, 1, 'Mardi', '14:00 - 16:00', 'LAB-NET', 'GI3-A', 'TP'),
(3, 1, 2, 'Jeudi', '10:00 - 12:00', 'B-104', 'DS-M1', 'Cours');

INSERT INTO Assignments (CourseId, Title, Description, DueDate, TotalPoints, Status) VALUES
(1, 'Mini projet React', 'Créer une interface responsive connectée à une API.', '2026-03-24 23:59:00', 20, 'Ouvert'),
(3, 'Notebook de classification', 'Comparer plusieurs algorithmes de classification.', '2026-03-27 23:59:00', 20, 'Ouvert');

INSERT INTO AssignmentSubmissions (AssignmentId, StudentId, FileUrl, Grade, Feedback, Status) VALUES
(1, 2, 'https://example.com/submission-react', 17, 'Bon travail, améliorer la validation.', 'Corrigé');

INSERT INTO Quizzes (CourseId, Title, Questions, DurationMinutes, Availability, StartAt, EndAt) VALUES
(1, 'Quiz JSX & Components', 12, 20, 'Disponible', '2026-03-18 08:00:00', '2026-03-20 23:00:00'),
(3, 'Quiz Régression', 15, 25, 'Planifié', '2026-03-21 08:00:00', '2026-03-23 23:00:00');

INSERT INTO QuizQuestions (QuizId, QuestionText, CorrectAnswer, Marks) VALUES
(1, 'À quoi sert un composant React ?', 'Réutiliser l''UI', 2),
(1, 'Que retourne JSX après transpilation ?', 'createElement', 2),
(2, 'Quel algorithme pour prédire une valeur continue ?', 'Régression linéaire', 2);

INSERT INTO ForumTopics (CreatedByUserId, CourseId, Forum, Title, IsClosed) VALUES
(1, NULL, 'Forum général', 'Bienvenue sur la plateforme universitaire', 0),
(2, 1, 'Développement Web Avancé', 'Questions sur le mini projet React', 0);

INSERT INTO ForumMessages (TopicId, CreatedByUserId, Content) VALUES
(1, 1, 'Bienvenue à tous sur le portail.'),
(2, 2, 'Posez ici vos questions sur le projet.'),
(2, 3, 'Peut-on utiliser Bootstrap avec React ?');

INSERT INTO AttendanceRecords (StudentId, CourseId, SessionDate, Status) VALUES
(1, 1, '2026-03-10', 'Présent'),
(1, 2, '2026-03-11', 'Absent'),
(1, 1, '2026-03-17', 'Présent');

INSERT INTO GradeRecords (StudentId, CourseId, Label, Score, MaxScore) VALUES
(1, 1, 'CC1', 15.50, 20.00),
(1, 2, 'TP Réseaux', 14.00, 20.00),
(2, 1, 'CC1', 17.00, 20.00);

INSERT INTO Notifications (UserId, Title, Message, Type, IsRead) VALUES
(3, 'Nouveau devoir', 'Le mini projet React a été publié.', 'Assignment', 0),
(3, 'Mise à jour emploi du temps', 'Le créneau du jeudi a été déplacé.', 'Timetable', 1),
(2, 'Nouvelle question forum', 'Un étudiant a posé une question sur le projet.', 'Forum', 0),
(1, 'Réservation laboratoire', 'Une nouvelle demande de réservation a été validée.', 'Laboratory', 0);

INSERT INTO LaboratoryReservations (LaboratoryId, ReservedByUserId, ReservationDate, TimeSlot, Purpose, Status) VALUES
(1, 3, '2026-03-19', '15:00 - 17:00', 'Travail de groupe projet IA', 'Validée');
