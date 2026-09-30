# Readers

A social reading platform where book lovers can connect, discover new books, and share their thoughts with a like-minded community.

Users can browse a curated collection of books, search by title, author, or genre, and interact through likes and comments on their favorite reads.

---

## Features

- **User accounts** — register, log in, and manage your profile via ASP.NET Core Identity
- **Book catalogue** — browse a grid of books with cover art, author, genre, and publication details
- **Search & filter** — find books by title, author, or genre through a slide-in search panel
- **Likes & comments** — engage with books by liking them and leaving comments
- **Responsive design** — clean, card-based layout that adapts to desktop and mobile

---

## Technologies

| Layer | Stack |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC (.NET 10) |
| Database | SQL Server (LocalDB) |
| ORM | Entity Framework Core |
| Auth | ASP.NET Core Identity |
| Front-end | Razor Views, HTML, CSS, minimal JavaScript |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (usually installed with Visual Studio)
- Git (optional, only if you clone the repository)

### Setup

1. **Clone or download the repository**

   Via Git:
   ```bash
   git clone GITHUB_PROJECT_URL
   Or download the ZIP and extract it.

### Navigate into the project folder

```bash
cd Readers/Readers
```

You should now see folders like `Controllers`, `Views`, `Models`, `Data`, `wwwroot`, and so on.

### Apply database migrations

Open a terminal in the project folder and run:

```bash
dotnet ef database update
```

This creates the database on your local SQL Server LocalDB instance and seeds it with an initial set of books.

### Run the application

```bash
dotnet run
```

### Open the site

The terminal will print a URL such as `https://localhost:5160`. Open it in your browser.

---

## Project Structure

```
Readers/
├── Controllers/        MVC controllers
├── Data/               DbContext, migrations, seeders
│   ├── DataModels/     Entity classes (Book, Author, Comment, Like)
│   ├── Migrations/     EF Core migrations
│   └── Seed/           Database seeding logic
├── Models/             ViewModels and DTOs
├── Views/              Razor views
│   ├── Books/          Book listing and search
│   ├── Home/           Homepage and privacy policy
│   └── Shared/         Layout and partials
├── wwwroot/            Static assets
│   ├── BookCovers/     Book cover images (WebP)
│   ├── css/            Stylesheets
│   └── js/             Client scripts
└── Program.cs          App configuration and service registration
```

---

## Database Schema (High-level)

**Tables:**

- **Books** — title, cover image, publishing house, genre, year, pages, author, likes, comments
- **Authors** — name, country
- **Comments** — text, associated book and user
- **Likes** — associated book and user
- **AspNetUsers** and related Identity tables — user accounts

**Relationships:**

- One Author → many Books
- One Book → many Comments and Likes
- One User → many Comments and Likes

---

## Roadmap / Known Limitations

- [ ] Multi-author support (currently one author per book)
- [ ] Full localization support (Bulgarian and English)
- [ ] User profiles with personal shelves
- [ ] Pagination on the books grid

---

## License

This project was created as part of a university course. Not intended for production use.