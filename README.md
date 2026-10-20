# Readers

**A place to discover books and share what you think about them.**

Readers is a web application for browsing a book catalogue, searching by title, author, or genre, and joining in through comments and likes. It is built with ASP.NET Core and uses ASP.NET Core Identity for user accounts.

> This repository is an educational project. Review and adapt its configuration, security, and operational practices before using it in production.

## Contents

- [Features](#features)
- [Technology](#technology)
- [Solution structure](#solution-structure)
- [Getting started](#getting-started)
- [Database and sample data](#database-and-sample-data)
- [Localization](#localization)
- [Current scope](#current-scope)

## Features

- Browse book details, including author, genre, publication year, publisher, page count, and cover art.
- Search by title and author with whitespace-normalized matching; filter by an exact genre.
- Register and sign in using ASP.NET Core Identity.
- Like books and create, edit, or delete your own comments when signed in.
- Use the localized application culture settings for English (United States) and Bulgarian (Bulgaria).

The catalogue currently displays at most **40 books**. Book browsing is available at `/Books`.

## Technology

| Area | Technology |
| --- | --- |
| Runtime | .NET 10 / ASP.NET Core |
| Web UI | ASP.NET Core MVC, Razor Views, and Identity Razor Pages |
| Data access | Entity Framework Core |
| Database | Microsoft SQL Server (LocalDB is configured for development) |
| Authentication | ASP.NET Core Identity |
| Front-end assets | HTML, CSS, JavaScript, Bootstrap, and jQuery |

## Solution structure

```text
Readers.slnx
├── Readers/                 Web application, controllers, views, Identity pages, and static assets
├── Readers.Core/            Book and comment service contracts and implementations
├── Readers.Data/            EF Core context, entities, migrations, and sample-data seeder
└── Readers.Common/          Shared data limits and application constants
```

Within the web project, `Controllers/` handles book and comment requests, `Views/` contains the MVC Razor UI, `Areas/Identity/Pages/` contains account pages, and `wwwroot/` holds static assets. The service layer is split between `Readers.Core/Contracts/` and `Readers.Core/Services/`; database entities and migrations live in `Readers.Data/`.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server, or SQL Server LocalDB for local development
- The Entity Framework Core CLI (`dotnet ef`) to apply migrations

### Restore and configure

From the repository root, restore the solution:

```sh
dotnet restore Readers.slnx
```

The application reads the `DevString` connection string first, then falls back to `DefaultConnection`. For local development, keep connection details out of source control by storing your connection string in User Secrets:

```sh
dotnet user-secrets set "ConnectionStrings:DevString" "<your SQL Server connection string>" --project Readers
```

The Web project already has a User Secrets ID. User Secrets are intended for development; configure connection strings through your deployment environment or secret store outside development.

If `dotnet ef` is not installed, install a version compatible with the EF Core packages in the solution:

```sh
dotnet tool install --global dotnet-ef --version 10.0.12
```

### Create or update the database

Run the existing EF Core migrations from the repository root:

```sh
dotnet ef database update --project Readers.Data --startup-project Readers --context ApplicationDbContext
```

### Run the application

```sh
dotnet run --project Readers
```

Open the HTTPS URL printed by the application in your browser. To compile the full solution without running it:

```sh
dotnet build Readers.slnx
```

## Database and sample data

`ApplicationDbContext` includes books, authors, comments, likes, and the ASP.NET Core Identity schema. EF Core migrations create and update the schema. A unique database index prevents the same user from liking the same book more than once.

Sample catalogue entries are defined in `Readers.Data/Seed/BookSeeder.cs`, but startup seeding is currently commented out in `Readers/Program.cs`. Applying migrations alone therefore **does not populate the catalogue**.

## Localization

The request localization middleware supports `en-US` and `bg-BG`, with `en-US` as the default. Culture can be selected with a query string, for example:

```text
?culture=bg-BG
```

The browser's `Accept-Language` header is also considered. Culture support is configured in the application; translated UI resources depend on the localization resources available in the project.

## Current scope

- Each book is associated with one author.
- The catalogue has a fixed display limit and does not currently provide pagination.
- Sample data is available in the repository but is not seeded automatically.
- The project is not presented as production-ready; deployment-specific hardening, configuration, and operational setup remain the responsibility of the deployment.
