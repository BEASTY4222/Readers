using Microsoft.EntityFrameworkCore;
using Readers.Common;
using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Core.Services
{
    public class BookService(ApplicationDbContext context) : IBookService
    {
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .OrderBy(b => b.Author.Name)
                .ThenBy(b => b.YearPublished)
                .Take(BookControllerLimits.MaxBooksToDisplay)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetGenres()
            => await context.Books.Select(b => b.Genre).Distinct().ToListAsync();


        public async Task<IEnumerable<Book>> GetSearchFields()
        {
            return await context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .ToListAsync();
        }

        public async Task<int> GetAuthorBookCount(int id)
            => await context.Books.CountAsync(b => b.AuthorId == id);

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> SearchAsync(string? title, string? author, string? genre)
        {
            var query = context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                var normalized = title.Replace(" ", "").ToLower();
                query = query.Where(b => b.Title.Replace(" ", "").ToLower().Contains(normalized));
            }

            if (!string.IsNullOrEmpty(author))
            {
                var normalized = author.Replace(" ", "").ToLower();
                query = query.Where(b => b.Author.Name.Replace(" ", "").ToLower().Contains(normalized));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genre == genre);
            }

            return await query
                .OrderBy(b => b.Author.Name)
                .Take(BookControllerLimits.MaxBooksToDisplay)
                .ToListAsync();
        }
    }
}
