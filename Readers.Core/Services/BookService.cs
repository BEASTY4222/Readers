using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readers.Common;
using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Core.Services
{
    /// <summary>
    /// Provides book data access and management services.
    /// Implements IBookService to handle all book-related database operations including retrieval, searching, and like management.
    /// </summary>
    public class BookService(ApplicationDbContext context) : IBookService
    {
        /// <summary>
        /// Retrieves all books from the database with their associated author, likes, and comments data.
        /// </summary>
        /// <returns>A collection of all books ordered by author name, then by year published, limited to MaxBooksToDisplay.</returns>
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

        /// <summary>
        /// Retrieves all distinct genre values from the books in the database.
        /// </summary>
        /// <returns>A collection of unique genre strings used for filter dropdowns.</returns>
        public async Task<IEnumerable<string>> GetGenres()
            => await context.Books.Select(b => b.Genre).Distinct().ToListAsync();


        /// <summary>
        /// Retrieves all books with their searchable metadata preloaded (author, likes, comments).
        /// Used internally to populate search indexes or retrieve full book data for filtering operations.
        /// </summary>
        /// <returns>A collection of all books with related entities fully loaded.</returns>
        public async Task<IEnumerable<Book>> GetSearchFields()
        {
            return await context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .ToListAsync();
        }

        /// <summary>
        /// Counts the total number of books in the database authored by a specific author.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <returns>The total count of books by the specified author.</returns>
        public async Task<int> GetAuthorBookCount(int id)
            => await context.Books.CountAsync(b => b.AuthorId == id);

        /// <summary>
        /// Retrieves a single book by its identifier, fully loaded with all related data.
        /// </summary>
        /// <param name="id">The unique identifier of the book to retrieve.</param>
        /// <returns>The book object with author, likes, and comments (including comment authors) if found; null otherwise.</returns>
        /// <remarks>This method eagerly loads comments with their associated users to avoid N+1 query problems in the view layer.</remarks>
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Searches for books based on optional filter criteria (title, author name, genre).
        /// All filters are optional and combined with AND logic; omitted filters are ignored.
        /// </summary>
        /// <param name="title">Optional search term for book title (case-insensitive, whitespace-insensitive).</param>
        /// <param name="author">Optional search term for author name (case-insensitive, whitespace-insensitive).</param>
        /// <param name="genre">Optional exact match for book genre.</param>
        /// <returns>A collection of matching books ordered by author name, limited to MaxBooksToDisplay.</returns>
        /// <remarks>
        /// Search normalization: titles and author names are converted to lowercase and whitespace is removed 
        /// to provide a more user-friendly search experience that tolerates spacing differences.
        /// </remarks>
        public async Task<IEnumerable<Book>> SearchAsync(string? title, string? author, string? genre)
        {
            var query = context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .AsQueryable();

            // Filter by normalized title if provided
            if (!string.IsNullOrEmpty(title))
            {
                var normalized = title.Replace(" ", "").ToLower();
                query = query.Where(b => b.Title.Replace(" ", "").ToLower().Contains(normalized));
            }

            // Filter by normalized author name if provided
            if (!string.IsNullOrEmpty(author))
            {
                var normalized = author.Replace(" ", "").ToLower();
                query = query.Where(b => b.Author.Name.Replace(" ", "").ToLower().Contains(normalized));
            }

            // Filter by exact genre match if provided
            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genre == genre);
            }

            return await query
                .OrderBy(b => b.Author.Name)
                .Take(BookControllerLimits.MaxBooksToDisplay)
                .ToListAsync();
        }

        /// <summary>
        /// Toggles the like status of a book for a specific user.
        /// If the user has already liked the book, the like is removed; otherwise, a new like is added.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book to toggle like for.</param>
        /// <param name="userId">The unique identifier of the user toggling the like.</param>
        /// <returns>True if the book is now in a liked state; false if the like has been removed.</returns>
        /// <remarks>
        /// This method uses an idempotent pattern: calling it multiple times with the same parameters
        /// will correctly toggle the like state back and forth.
        /// </remarks>
        public async Task<bool> ToggleLikeAsync(int bookId, string userId)
        {
            // Check if the user has already liked this book
            var existingLike = await context.Likes
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.UserId == userId);

            if (existingLike != null)
            {
                // User already liked the book → remove the like (unlike)
                context.Likes.Remove(existingLike);
                await context.SaveChangesAsync();
                return false;   // new state: not liked
            }

            // User hasn't liked the book yet → add the like
            context.Likes.Add(new Like
            {
                BookId = bookId,
                UserId = userId
            });
            await context.SaveChangesAsync();
            return true;   // new state: liked
        }

        /// <summary>
        /// Determines whether a specific user has liked a particular book.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>True if the user has liked the book; false otherwise.</returns>
        public async Task<bool> UserHasLikedAsync(int bookId, string userId)
        {
            return await context.Likes
                .AnyAsync(l => l.BookId == bookId && l.UserId == userId);
        }
    }
}
