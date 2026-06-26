# TuneVault - Media Streaming Web Application

TuneVault is a Spotify-like media streaming monorepo application. It features a modern React SPA frontend and a C# .NET Core Clean Architecture backend with SQL Server persistence, JWT authentication, and real-time notifications via SignalR.

---

## 📂 Project Structure

```
sharppro/
├── backend/                  # ASP.NET Core 8 Web API (Clean Architecture)
│   ├── src/
│   │   ├── TuneVault.Domain/         # Core domain entities, enums, & domain events
│   │   ├── TuneVault.Application/    # Use cases, MediatR command/query handlers, & validation
│   │   ├── TuneVault.Infrastructure/ # EF Core, SQL Server context, Identity, & DbSeeder
│   │   └── TuneVault.API/            # Controllers, middleware, JWT auth, & SignalR Hubs
│   └── TuneVault.sln         # Backend solution file
│
├── frontend/                 # React 18 Single Page Application
│   ├── src/
│   │   ├── components/       # Reusable components & AppShell layout
│   │   ├── pages/            # View pages (Library, Notifications, Share Inbox, Video, etc.)
│   │   ├── services/         # API client & auth services
│   │   ├── stores/           # Zustand state management (audio player, notification state)
│   │   └── types.ts          # Centralized TypeScript type declarations
│   ├── package.json          # Node dependencies and npm scripts
│   └── vite.config.ts        # Vite configuration
│
└── docker-compose.yml        # Docker composition for database (SQL Server)
```

---

## 🛠️ Prerequisites

Ensure you have the following installed on your machine:

- **Node.js**: `v20.x` or later (packaged with `npm`)
- **.NET SDK**: `8.0` or later
- **Docker Desktop**: For running the SQL Server database in a container

---

## 🚀 Getting Started (Step-by-Step)

### Step 1: Start the SQL Server Database

TuneVault uses Microsoft SQL Server for data persistence. A pre-configured database service is defined in `docker-compose.yml`.

1. Ensure **Docker Desktop** (or the Docker daemon) is running on your machine.
2. In the repository root, run the following command to start SQL Server in the background:
   ```bash
   docker compose up -d
   ```
3. This spins up the SQL Server instance listening on `localhost:1433`.

> [!NOTE]
> The database connection details are already configured in [appsettings.json](SharpPro/backend/src/TuneVault.API/appsettings.json) and [appsettings.Development.json](SharpPro/backend/src/TuneVault.API/appsettings.Development.json) to match the Docker credentials. No configuration changes are required out-of-the-box.

---

### Step 2: Build and Run the Backend API

1. Navigate to the backend directory:
   ```bash
   cd backend
   ```
2. Restore the dependencies and build the solution:
   ```bash
   dotnet restore TuneVault.sln
   dotnet build TuneVault.sln
   ```
3. Run the API project:
   ```bash
   dotnet run --project src/TuneVault.API/TuneVault.API.csproj
   ```
4. The server will start up on `http://localhost:5237` (HTTP) and `https://localhost:7065` (HTTPS).
5. **Verification**: Open [Swagger UI](http://localhost:5237/swagger) in your browser to explore and interact with the endpoints.

> [!TIP]
> **Automatic Migration & Seeding**: On application startup, the API automatically triggers EF Core migrations (`dbContext.Database.MigrateAsync()`) and seeds the database with initial user, media items, and playlists. You do **not** need to run database migrations manually.

---

### Step 3: Run the Frontend Application

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```
2. Install the frontend dependencies:
   ```bash
   npm install
   ```
3. Start the Vite local development server:
   ```bash
   npm run dev
   ```
4. The frontend will launch at: [http://localhost:5173](http://localhost:5173)

---

## 👤 Seeded Test Credentials

To start testing and exploring the application right away, use the following seeded account:

- **Email**: `testuser@tunevault.local`
- **Password**: `Password123!`

### Initial Seeded Data Included:

- **Starter User**: `starter@tunevault.local`
- **Tracks**: Preloaded audio tracks like _Lo-Fi Chill_, _Synthwave Ride_, and _Starter Track_.
- **Playlists**: _Test Public Playlist_, _Test Private Playlist_, and _Getting Started_.

---

## 🎵 Features to Explore & Verify

- **Spotify-like UI**: Responsive dark shell layout with sidebar navigation and a floating bottom player bar.
- **Audio & Video Playback**: Click **Play now** on any audio track. For video files, selecting them launches a full-screen overlay player.
- **Playlist Management**: View, play, and create playlists from the **Library** page, or delete tracks from existing playlists.
- **Media Upload**: Upload custom MP3 (audio) or MP4 (video) files directly to the server. Uploaded files are stored in `backend/src/TuneVault.API/Uploads` and streamed dynamically.
- **Secure Sharing**: Share tracks and playlists with other users.
- **Real-Time Notification Hub**: Sharing actions trigger immediate alerts via a SignalR persistent websocket connection. Click the bell icon or visit **Notifications** to see shared tracks.

---

## 🛠️ Diagnostics & Troubleshooting

### Docker Database Connection Issues

If the API fails to connect to the database, verify that:

1. Docker Desktop is running.
2. The container is up and running. Check status with:
   ```bash
   docker ps
   ```
3. The port `1433` is not being occupied by another local instance of SQL Server.

### Resetting the Database

To clear all data and start fresh:

1. Stop and remove the Docker container and volumes:
   ```bash
   docker compose down -v
   ```
2. Re-start the Docker container:
   ```bash
   docker compose up -d
   ```
3. Run the backend API again to automatically apply migrations and seed fresh data.

### Frontend API URL Configuration

The API URL defaults to `http://localhost:5237/api`. To change it, modify the `VITE_API_BASE_URL` environment variable inside [frontend/.env](/SharpPro/frontend/.env).

---

## 🙏 Credits, Citations & Templates

### 📚 Frameworks & Libraries

#### Backend
| Library / Framework | Purpose | License |
|---|---|---|
| [ASP.NET Core 8](https://docs.microsoft.com/aspnet/core) | Web API framework | MIT |
| [MediatR](https://github.com/jbogard/MediatR) by Jimmy Bogard | CQRS mediator pattern | Apache 2.0 |
| [Entity Framework Core](https://github.com/dotnet/efcore) | ORM for SQL Server | MIT |
| [FluentValidation](https://docs.fluentvalidation.net) | Request validation pipeline | Apache 2.0 |
| [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity) | User management & password hashing | MIT |
| [Microsoft.AspNetCore.SignalR](https://github.com/dotnet/aspnetcore) | Real-time WebSocket hub | MIT |
| [System.IdentityModel.Tokens.Jwt](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet) | JWT token generation & validation | MIT |

#### Frontend
| Library / Framework | Purpose | License |
|---|---|---|
| [React 18](https://react.dev) by Meta | UI component framework | MIT |
| [Vite](https://vitejs.dev) by Evan You | Build tool & dev server | MIT |
| [TypeScript](https://www.typescriptlang.org) by Microsoft | Static typing for JavaScript | Apache 2.0 |
| [Tailwind CSS](https://tailwindcss.com) by Adam Wathan | Utility-first CSS framework | MIT |
| [Zustand](https://github.com/pmndrs/zustand) by Pmndrs | Lightweight state management | MIT |
| [Axios](https://axios-http.com) | HTTP client with interceptors | MIT |
| [React Router v6](https://reactrouter.com) | Client-side routing | MIT |
| [@microsoft/signalr](https://www.npmjs.com/package/@microsoft/signalr) | SignalR client for frontend | MIT |

#### DevOps & Infrastructure
| Tool | Purpose |
|---|---|
| [Docker](https://www.docker.com) | Container runtime |
| [Docker Compose](https://docs.docker.com/compose) | Multi-container orchestration |
| [Microsoft SQL Server](https://www.microsoft.com/sql-server) | Relational database |

---

### 🎨 Design Inspiration

- **UI/UX**: Inspired by [Spotify](https://open.spotify.com) — dark shell layout, floating bottom player, sidebar navigation, and green accent colors.
- **Dark Mode Design System**: Color palette adapted from [Zinc / Tailwind CSS dark palettes](https://tailwindcss.com/docs/customizing-colors).

---

### 📄 Report Template

The assignment report ([`Report/Assignment_report.tex`](Report/Assignment_report.tex)) is written in **LaTeX** using a template provided by **Trường Đại học Sài Gòn (SGU), Khoa Công Nghệ Thông Tin**.

| LaTeX Package | Purpose |
|---|---|
| `vntex` | Vietnamese language support |
| `fancyhdr` | Custom header & footer |
| `tabularx`, `booktabs` | Formatted tables |
| `algorithm2e` | Algorithm typesetting |
| `listings` | Source code highlighting |
| `hyperref` | Clickable links & references |
| `tikz`, `mdframed` | Diagrams & highlighted boxes |
| `lastpage` | Total page count in footer |

---

### 👥 Team

| Name | Student ID |
|---|---|
| Trần Đăng Nguyên | 3121411152 |
| Ngô Thành Đạt | 3124410056 |
| Trịnh Gia Bảo | 3124560011 |
| Nguyễn Minh Triệu | 3124410375 |
| Nguyễn Lê Thắng | 3124410330 |

**Instructor**: Từ Lãng Phiêu  
**Course**: Phát triển phần mềm mã nguồn mở — Niên khóa 2023–2024  
**Institution**: Trường Đại học Sài Gòn (SGU)

---

### ⚖️ License

This project is developed as an academic assignment and is not licensed for commercial use. All third-party libraries retain their respective open-source licenses as listed above.
