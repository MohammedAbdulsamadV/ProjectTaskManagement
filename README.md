# ProjectTaskManagement

This repository contains a full Project Task Management system with two parts:

- Frontend: an Angular application generated with Angular CLI.
- Backend: an ASP.NET Core Web API (Clean Architecture) with CQRS, JWT authentication, and Docker support.

## Frontend (Angular)

Development server

```bash
cd <frontend-folder-or-root>
ng serve
```

Open http://localhost:4200/ when the server is running. The application reloads on source changes.

Build

```bash
ng build
```

Run unit tests

```bash
ng test
```

## Backend (ASP.NET Core Web API)

A scalable backend built with .NET 10 using Clean Architecture and EF Core.

Tech stack and features:
- ASP.NET Core Web API (.NET 10)
- Clean Architecture (API, Application, Domain, Infrastructure)
- Entity Framework Core
- SQL Server (Docker)
- JWT Authentication
- CQRS pattern
- AutoMapper
- Swagger / OpenAPI
- Docker & Docker Compose

API Endpoints (examples)
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/projects`
- `POST /api/projects`
- `GET /api/tasks/project/{projectId}`

## Development

- Backend: open the solution in Visual Studio / Rider / VS Code. Use `dotnet run` or Docker Compose.
- Frontend: run `ng serve` in the Angular project folder.

## Notes

This combined repository now contains both frontend and backend source trees. See the relevant subfolders for more details and README files specific to each part.
