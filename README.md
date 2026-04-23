# University Management - Version finale

Projet full-stack de gestion universitaire avec séparation des espaces Administration / Professeur / Étudiant.

## Contenu
- `frontend/` : application React + Bootstrap + design responsive
- `backend/` : API ASP.NET Core (.NET 10) + EF Core + MySQL
- `database/` : script SQL pour MySQL/XAMPP

## Base de données
Importer `database/university_management_mysql_xampp.sql` dans phpMyAdmin.

## Backend
```bash
cd backend
dotnet restore UniversityManagement.sln
dotnet run --project UniversityManagement.Api.csproj
```

## Frontend
```bash
cd frontend
npm install
copy .env.example .env
npm run dev
```

## Comptes démo
- admin@university.ma / 123456
- prof@university.ma / 123456
- student@university.ma / 123456
