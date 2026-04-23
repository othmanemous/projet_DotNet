# Backend University Management

Backend ASP.NET Core Web API (.NET 10) prêt pour Visual Studio et MySQL XAMPP.

## Base de données
1. Démarrer Apache et MySQL dans XAMPP.
2. Importer le fichier `../database/university_management_mysql_xampp.sql` dans phpMyAdmin.
3. Vérifier que la base `university_management_db` existe.

## Lancement
```bash
cd backend
dotnet restore UniversityManagement.sln
dotnet run --project UniversityManagement.Api.csproj
```

## URLs utiles
- Swagger: `https://localhost:7164/swagger`
- Health: `https://localhost:7164/api/health`

## Comptes de démonstration
- admin@university.ma / 123456
- prof@university.ma / 123456
- student@university.ma / 123456
