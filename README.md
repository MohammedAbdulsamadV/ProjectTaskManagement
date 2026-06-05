📌 Project Task Management API

A scalable backend system built with ASP.NET Core Web API (.NET 10) following Clean Architecture, implementing CQRS, JWT Authentication and Docker support.

🚀 Tech Stack
ASP.NET Core Web API (.NET 10)
Clean Architecture
Entity Framework Core
SQL Server (Docker)
JWT Authentication
CQRS Pattern (MediatR optional)
AutoMapper
Swagger / OpenAPI
Docker & Docker Compose
🧱 Architecture

Project follows Clean Architecture:

ProjectTask.API
ProjectTask.Application
ProjectTask.Domain
ProjectTask.Infrastructure
Layers Responsibility:
Domain → Entities & Core Business Rules
Application → CQRS, DTOs, Interfaces
Infrastructure → EF Core, Repositories
API → Controllers, Middleware, Swagger
🔐 Features
#Authentication
Register User
Login User
JWT Token Generation
#Projects Module
Create Project
Get All Projects
Get Project By Id
Update Project
Delete Project
#Tasks Module
Create Task
Update Task Status
Get Tasks By Project
Delete Task
📦 API Endpoints
Auth
-POST /api/auth/register
-POST /api/auth/login
Projects
-GET    /api/projects
-GET    /api/projects/{id}
-POST   /api/projects
-PUT    /api/projects/{id}
-DELETE /api/projects/{id}
Tasks
-GET    /api/tasks/project/{projectId}
-POST   /api/tasks
-PUT    /api/tasks/{id}/status
-DELETE /api/tasks/{id}
🧠 Design Patterns Used
*Repository Pattern
*Unit of Work
*CQRS Pattern
*Dependency Injection
*Global Exception Handling
*DTO Mapping (AutoMapper)
🚀 Performance Features
=Retry Policy for SQL Server
=Async/Await everywhere
=Clean separation of concerns
