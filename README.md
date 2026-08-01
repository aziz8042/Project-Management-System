# 📋 WorkClever — Project Management System

A **Trello/Jira-style project management application** with a **.NET 6 Web API** backend and a **React + TypeScript (Vite)** frontend. WorkClever lets teams organize work into projects, boards, and columns, manage tasks with assignees, comments, attachments, custom fields, and task relations — all through a modern drag-and-drop board UI.

---

## 📚 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture Overview](#-architecture-overview)
- [Project Structure](#-project-structure)
- [Prerequisites](#-prerequisites)
- [Getting Started (Step-by-Step)](#-getting-started-step-by-step)
  - [1. Clone the Repository](#1-clone-the-repository)
  - [2. Backend Setup (.NET API)](#2-backend-setup-net-api)
  - [3. Frontend Setup (React Client)](#3-frontend-setup-react-client)
  - [4. Running the Application](#4-running-the-application)
- [Default Seeded Accounts](#-default-seeded-accounts)
- [Configuration](#-configuration)
- [API Overview](#-api-overview)
- [Troubleshooting](#-troubleshooting)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

- 🗂️ **Projects, Boards & Columns** — organize work hierarchically (Project → Board → Column → Task)
- ✅ **Task Management** — create, update, and order tasks with drag-and-drop
- 👥 **Task Assignees** — assign one or more users to a task
- 💬 **Task Comments** — discuss tasks inline
- 📎 **Attachments** — upload files to tasks
- 🔗 **Task Relations** — link tasks together (e.g. "Blocks" / "Blocked by") via configurable relation types
- 🧩 **Custom Fields** — extend tasks with project-specific custom fields
- 📜 **Task Change Log** — track history of changes made to a task
- 🔔 **User Notifications** — in-app notifications for task activity
- 🔐 **Authentication & Roles** — JWT-based auth with ASP.NET Core Identity and role-based authorization (e.g. Admin)
- 🌍 **Site Settings** — configurable timezone and date/time formats
- 🖼️ **User Avatars & Preferences** — profile customization per user

---

## 🛠 Tech Stack

**Backend**
- ASP.NET Core 6 Web API (C#)
- Entity Framework Core 6 (SQLite by default; MySQL/Pomelo support included)
- ASP.NET Core Identity (user & role management)
- JWT Bearer Authentication
- Newtonsoft.Json / System.Text.Json hybrid serialization
- CountryData.Standard (country/locale utilities)

**Frontend**
- React 18 + TypeScript
- Vite (dev server & build tool)
- Redux Toolkit + Redux-Saga (state management & side effects)
- Ant Design (`antd`) + `@ant-design/pro-components` (UI library)
- Elastic UI (`@elastic/eui`) components
- Axios (HTTP client)
- React Router v6
- `@ozgurrgul/dragulax` (drag-and-drop board interactions)
- date-fns / dayjs / moment (date handling)
- styled-components + Emotion (styling)

---

## 🏛 Architecture Overview

The backend (`WorkCleverSolution`) follows a straightforward layered structure:

- **Controllers** — thin ASP.NET Core MVC/API controllers that receive requests and delegate to services.
- **Services** — business logic for each domain area (Projects, Boards, Columns, Tasks, Comments, Relations, Users, Notifications, etc.), all exposed through interfaces and aggregated behind a single `IServices` facade.
- **Data** — EF Core entity classes and the `ApplicationDbContext`, plus an `Identity` sub-folder for user/role/permission models.
- **Dto** — request/response Data Transfer Objects, organized by feature area (`Project`, `User`, `Auth`, `Global`, etc.).
- **Migrations** — EF Core code-first migrations for schema versioning.
- **Attributes** — cross-cutting concerns: authorization policies, generic filters, and model validation.

The frontend (`ClientApp`) is a single-page React app:

- **`pages/`** — top-level routed pages (`auth`, `project`, `manage`) including the main Kanban `BoardPage`.
- **`services/endpoints/`** — one file per backend resource, defining typed API calls.
- **`slices/`** — Redux Toolkit slices per domain (`auth`, `board`, `project`, `taskDetail`, `navigate`, `app`).
- **`components/`** — shared and feature-specific UI components.
- **`layout/`** — app shell/navigation layout.

The two apps communicate purely over HTTP/JSON — the React app is a fully independent client that talks to the API via a configurable base URL (`VITE_APP_API_URL`).

---

## 🗂 Project Structure

```text
Project-Management-System/
│
├── WorkCleverSolution/                   # .NET 6 Web API backend
│   ├── Attributes/
│   │   ├── Authorization/                 # Custom authorization attributes/policies
│   │   ├── Generic/                       # Generic action/exception filters
│   │   └── Validation/                    # Model validation attributes
│   │
│   ├── Controllers/                       # API controllers (one per resource)
│   │   ├── AuthController.cs
│   │   ├── BaseApiController.cs
│   │   ├── BoardController.cs
│   │   ├── ColumnController.cs
│   │   ├── CustomFieldController.cs
│   │   ├── DictionaryController.cs
│   │   ├── ProjectController.cs
│   │   ├── ServiceResult.cs
│   │   ├── SiteSettingsController.cs
│   │   ├── TaskCommentController.cs
│   │   ├── TaskController.cs
│   │   ├── TaskRelationController.cs
│   │   ├── TaskRelationTypeDefController.cs
│   │   ├── UserController.cs
│   │   └── UserNotificationController.cs
│   │
│   ├── Data/                              # EF Core entities & DbContext
│   │   ├── Identity/                      # User.cs, Roles.cs, Permissions.cs, UserPreference.cs
│   │   ├── ApplicationDbContext.cs
│   │   ├── Project.cs / Board.cs / Column.cs / TaskItem.cs
│   │   ├── TaskAssignee.cs / TaskComment.cs / TaskAttachment.cs
│   │   ├── TaskRelation.cs / TaskRelationTypeDef.cs / TaskParentRelation.cs
│   │   ├── TaskChangeLog.cs / CustomField.cs / BoardView.cs
│   │   ├── UserEntityAccess.cs / SiteSettings.cs
│   │   ├── Repository.cs / TimeAwareEntity.cs
│   │
│   ├── Dto/                                # Request/response DTOs, grouped by feature
│   │   ├── Auth/  Global/  User/
│   │   └── Project/ (incl. Board/, Column/, Task/ subfolders)
│   │
│   ├── Extensions/
│   │   └── UserExtensions.cs
│   │
│   ├── Migrations/                         # EF Core migrations
│   │
│   ├── Services/                           # Business logic implementations
│   │   ├── AuthService.cs / UserService.cs
│   │   ├── ProjectService.cs / BoardService.cs / BoardViewService.cs / ColumnService.cs
│   │   ├── TaskService.cs / TaskCommentService.cs / TaskRelationService.cs
│   │   ├── TaskAssigneeService.cs / TaskChangeLogService.cs / TaskRelationTypeDefService.cs
│   │   ├── CustomFieldService.cs / SiteSettingsService.cs / FileUploadService.cs
│   │   ├── UserEntityAccessManagerService.cs / UserNotificationService.cs
│   │   └── Services.cs                     # IServices facade aggregating all services
│   │
│   ├── Utils/
│   │   ├── CountryUtils.cs
│   │   └── ReflectionUtils.cs
│   │
│   ├── Properties/
│   │   └── launchSettings.json
│   │
│   ├── ApplicationException.cs
│   ├── Constants.cs                        # JWT key/issuer/expiry
│   ├── DBSeeder.cs                         # Seeds demo users, project, boards, columns
│   ├── Program.cs                          # App composition root & middleware pipeline
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── WorkCleverSolution.csproj
│
├── ClientApp/                              # React + TypeScript (Vite) frontend
│   ├── public/                             # Static assets
│   ├── src/
│   │   ├── components/
│   │   │   ├── shared/                     # Reusable shared components
│   │   │   └── user/                       # User-related components (avatar upload, etc.)
│   │   ├── css/                            # Global styles
│   │   ├── hooks/                          # Custom React hooks
│   │   ├── layout/                         # App shell / navigation layout
│   │   ├── pages/
│   │   │   ├── auth/                       # Login / register pages
│   │   │   ├── manage/                     # GlobalSettingsPage + admin components
│   │   │   └── project/                    # ProjectPage, BoardPage, ProjectLayout + board components
│   │   ├── services/
│   │   │   ├── endpoints/                  # Typed API endpoint definitions per resource
│   │   │   ├── optimistic/                 # Optimistic-update helpers
│   │   │   └── api.ts                      # Base Axios/RTK-style API client
│   │   ├── slices/                         # Redux Toolkit slices (auth, board, project, taskDetail, navigate, app)
│   │   ├── types/                          # Shared TypeScript types
│   │   └── constants.ts                    # API_URL / BACKEND_URL from env vars
│   ├── .env                                # VITE_APP_API_URL configuration
│   ├── index.html
│   ├── package.json
│   ├── tsconfig.json
│   ├── vite.config.ts
│   ├── craco.config.js
│   ├── eslint.config.js
│   └── LICENSE.md
│
├── global.json                             # Pins .NET SDK version
└── WorkCleverSolution.sln
```

---

## ✅ Prerequisites

| Tool | Version | Notes |
|---|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | **6.0.x** (pinned via `global.json`, `6.0.425`) | Required to build/run the API |
| [Node.js](https://nodejs.org/) | 18.x or later | Required to run the Vite dev server |
| [Yarn](https://yarnpkg.com/) | Latest | Project ships with `yarn.lock` |
| Git | Latest | To clone the repository |
| SQLite | *(bundled via EF Core provider)* | Default database — no separate install needed |
| MySQL *(optional)* | 8.x | Alternative DB provider is already referenced (`Pomelo.EntityFrameworkCore.MySql`) if you prefer MySQL over SQLite |

---

## 🚀 Getting Started (Step-by-Step)

### 1. Clone the Repository

```bash
git clone https://github.com/<your-username>/Project-Management-System.git
cd Project-Management-System
```

### 2. Backend Setup (.NET API)

Restore and build the solution:

```bash
dotnet restore WorkCleverSolution.sln
dotnet build WorkCleverSolution.sln
```

By default, the API uses **SQLite** with the connection string in `WorkCleverSolution/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=db.db;"
  }
}
```

No manual migration step is required — `Program.cs` calls `dbContext.Database.EnsureCreated()` and `DbSeeder.Seed(...)` automatically on startup, creating the SQLite file, applying the schema, and inserting demo data the first time you run the app.

> 💡 If you'd rather use MySQL, swap the `AddDbContext` provider in `Program.cs` from `UseSqlite` to `UseMySql`/`UseMySQL` (Pomelo) and update the connection string accordingly — both packages are already referenced in `WorkCleverSolution.csproj`.

### 3. Frontend Setup (React Client)

```bash
cd ClientApp
yarn install
```

Confirm (or create) the `.env` file so the frontend knows where the API lives:

```env
VITE_APP_API_URL="http://localhost:5001"
```

### 4. Running the Application

**Run the API** (from the project root, in one terminal):

```bash
cd WorkCleverSolution
dotnet run
```

The API starts at **http://localhost:5001** (see `Properties/launchSettings.json`).

**Run the React client** (in a second terminal):

```bash
cd ClientApp
yarn dev
```

The client starts at **http://localhost:3000** via Vite and talks to the API using the `VITE_APP_API_URL` you configured.

Open **http://localhost:3000** in your browser and log in with one of the seeded demo accounts below. 🎉

---

## 🔑 Default Seeded Accounts

On first run, `DBSeeder.cs` automatically creates a demo dataset — a Site Settings record, an `Admin` role, four demo users, a sample project (`My Project`) with three boards and several columns, and two task relation types (`Blocks` / `Blocked by`).

| Name | Email | Password | Role |
|---|---|---|---|
| Ozgur GUL | `admin@admin.com` | `Asd32!#` | Admin |
| Sergey Brin | `sergey@brin.com` | `Asd32!#` | — |
| Larry Page | `larry@page.com` | `Asd32!#` | — |
| Mark Zuckerberg | `mark@zuckerberg.com` | `Asd32!#` | — |

> ⚠️ **Security note:** These are development/demo credentials defined in `DBSeeder.cs`. Change or remove them before deploying to a real environment, and never reuse this password in production.

---

## ⚙️ Configuration

| Key | Location | Description |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `WorkCleverSolution/appsettings.json` | Database connection string (SQLite by default) |
| `Constants.JwtKey` / `JwtIssuer` / `JwtExpireDays` | `WorkCleverSolution/Constants.cs` | JWT signing key, issuer, and token expiry |
| `VITE_APP_API_URL` | `ClientApp/.env` | Base URL the React app uses to reach the API |

> ⚠️ **Security note:** `Constants.JwtKey` ships with a placeholder development value. Replace it with a strong, secret value (ideally loaded from environment variables or a secrets manager) before deploying publicly.

CORS is wide-open by default in `Program.cs` (`AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials()`), which is convenient for local development but should be tightened for production.

---

## 📡 API Overview

The API is organized into resource-based controllers, all backed by services behind a single `IServices` facade:

| Controller | Responsibility |
|---|---|
| `AuthController` | Registration, login, JWT issuance |
| `UserController` | User CRUD, roles, avatars, preferences |
| `ProjectController` | Project CRUD & project assignees |
| `BoardController` | Boards within a project |
| `ColumnController` | Columns within a board |
| `TaskController` | Task CRUD, ordering, assignees, attachments |
| `TaskCommentController` | Comments on tasks |
| `TaskRelationController` / `TaskRelationTypeDefController` | Task-to-task relations and relation type definitions |
| `CustomFieldController` | Project-level custom fields |
| `SiteSettingsController` | Global site settings (timezone, date formats) |
| `UserNotificationController` | In-app user notifications |
| `DictionaryController` | Lookup/reference data (e.g. countries) |

---

## 🐛 Troubleshooting

- **API won't start / DB errors** — delete the generated `db.db` SQLite file and restart; `EnsureCreated()` + `DbSeeder` will recreate and reseed it.
- **Frontend can't reach the API / CORS errors** — confirm `ClientApp/.env` has `VITE_APP_API_URL` pointing to the exact URL the API is running on (including protocol and port).
- **Login fails with seeded accounts** — make sure the backend seeded successfully (check the console for "Site settings created" / "Roles created" logs) and that you're using the exact email/password combinations above.
- **Port conflicts** — change the backend port in `WorkCleverSolution/Properties/launchSettings.json`, or the frontend port in `vite.config.ts` (`server.port`), if `5001`/`3000` are already in use.
- **File upload issues** — uploaded task attachments and avatars are handled by `FileUploadService`/`RegularFileUploader`; ensure the API process has write access to its working directory.

---

## 🤝 Contributing

Contributions are welcome!

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License** — see `ClientApp/LICENSE.md` for details.

---

<p align="center">Made with ❤️ using .NET 6, React, TypeScript, and Ant Design</p>
