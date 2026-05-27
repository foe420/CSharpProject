# TuneVault - Media Streaming Web Application Starter

TuneVault is a **starter template** for a Spotify-like media streaming application built as a C# .NET final project.

## Monorepo Structure

- `backend/` - ASP.NET Core 8 Web API (Clean Architecture)
- `frontend/` - React 18 + TypeScript + Vite + Tailwind CSS

## Backend Architecture (Clean Architecture)

`backend/src` contains four projects:

- `TuneVault.Domain` - entities and core enums
- `TuneVault.Application` - MediatR commands/queries, FluentValidation, pipeline behaviors
- `TuneVault.Infrastructure` - EF Core SQL Server context, repositories, seed data
- `TuneVault.API` - controllers, JWT auth, Swagger, SignalR hub

## Prerequisites

- .NET SDK 8+
- Node.js 20+
- SQL Server (local or Docker)

## Run with Docker (SQL Server only)

```bash
docker compose up -d
```

This starts SQL Server on `localhost:1433`.

## Backend Setup

1. Navigate to backend:
   ```bash
   cd backend
   ```
2. Update connection string in:
   - `/tmp/workspace/foe420/CSharpProject/backend/src/TuneVault.API/appsettings.json`
3. Restore/build/run:
   ```bash
   dotnet restore TuneVault.sln
   dotnet build TuneVault.sln
   dotnet run --project src/TuneVault.API/TuneVault.API.csproj
   ```
4. Open Swagger:
   - `https://localhost:5001/swagger` (or launch profile URL)

## Seeding Initial Data

On API startup, `DbSeeder` runs automatically and inserts starter:

- 1 user (`starter@tunevault.local`)
- 1 media item (`Starter Track`)
- 1 playlist (`Getting Started`)

Seeder location:
- `/tmp/workspace/foe420/CSharpProject/backend/src/TuneVault.Infrastructure/Persistence/DbSeeder.cs`

## Frontend Setup

1. Navigate to frontend:
   ```bash
   cd frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Optional API URL override:
   - Create `.env` in `frontend/`
   - Set `VITE_API_BASE_URL=https://localhost:5001/api`
4. Run dev server:
   ```bash
   npm run dev
   ```

## Starter Features Included

- JWT authentication starter endpoint (`POST /api/auth/login`)
- Media library endpoints (`GET /api/media`, `POST /api/media`)
- SignalR notification hub (`/hubs/notifications`)
- Spotify-like dark UI shell (sidebar, main content, bottom player)

## Next Suggested Extensions

- Upload and stream audio/video files
- Playlist management commands/queries
- Share media with secure links
- Real-time playback/notification events via SignalR
- AI-assisted recommendations/search
