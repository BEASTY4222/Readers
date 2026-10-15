using Microsoft.AspNetCore.Mvc;
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

        public async Task<bool> ToggleLikeAsync(int bookId, string userId)
        {
            // Look for an existing like by this user on this book
            var existingLike = await context.Likes
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.UserId == userId);

            if (existingLike != null)
            {
                // Already liked → unlike
                context.Likes.Remove(existingLike);
                await context.SaveChangesAsync();
                return false;   // new state: not liked
            }

            // Not liked yet → like it
            context.Likes.Add(new Like
            {
                BookId = bookId,
                UserId = userId
            });
            await context.SaveChangesAsync();
            return true;   // new state: liked
        }

        public async Task<bool> UserHasLikedAsync(int bookId, string userId)
        {
            return await context.Likes
                .AnyAsync(l => l.BookId == bookId && l.UserId == userId);
        }
    }
}
