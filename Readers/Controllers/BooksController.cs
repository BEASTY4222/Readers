using Microsoft.AspNetCore.Authorization; // for AsAsyncEnumerable() if available via package
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Readers.Common;
using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;
using Readers.Web.ViewModels;
using System.Linq;

namespace Readers.Web.Controllers
{
    /// <summary>
    /// Handles HTTP requests related to book browsing, searching, viewing details, and managing user likes.
    /// Provides endpoints for displaying books, filtering by search criteria, and toggling like status for authenticated users.
    /// </summary>
    public class BooksController(IBookService bookService, UserManager<ApplicationUser> userManager) : Controller
    {
        /// <summary>
        /// Displays the index page with all available books and genres for filtering.
        /// </summary>
        /// <returns>A view containing a list of all books and available genres for user selection.</returns>
        /// <remarks>
        /// This action retrieves all books from the database (limited to MaxBooksToDisplay as per BookControllerLimits).
        /// Books are returned ordered by author name and year published. The view also receives a list of all unique genres
        /// to enable filtering functionality.
        /// </remarks>
        [Route("/Books")]
        public async Task<IActionResult> Index()
        {
            // Retrieve all books from database (respects MaxBooksToDisplay limit as of 26.09.2026)
            List<Book> books = (await bookService.GetAllBooks()).ToList();

            // Map Book entities to BookViewModels for safe presentation to the view layer
            List<BookViewModel> bookViewModels = books
            .Select(b => new BookViewModel
            {
                Id = b.Id,
                CoverImagePath = b.CoverImagePath,
                Title = b.Title,
                Author = b.Author,
                Likes = b.Likes,
                Genre = b.Genre,
                Comments = b.Comments
            })
            .ToList();

            return View(new BookIndexViewModel
            {
                Books = bookViewModels,
                Genres = (await bookService.GetGenres()).ToList()
            });
        }

        /// <summary>
        /// Searches for books based on title, author name, and/or genre filters.
        /// </summary>
        /// <param name="model">The search input model containing optional title, author, and genre filters.</param>
        /// <returns>A view containing the filtered list of books and all available genres.</returns>
        /// <remarks>
        /// Search is case-insensitive and ignores whitespace differences in title and author names for better user experience.
        /// Returns results ordered by author name. If no filters are provided, returns all available books.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Search(SearchFormInputModel model)
        {
            // Execute search query with provided filters (title, author, genre)
            var books = await bookService.SearchAsync(
                model.SearchedTitle,
                model.SearchedAuthor,
                model.SearchedGenre);

            // Transform domain models to view models for safe presentation
            var viewModel = new BookIndexViewModel
            {
                Books = books.Select(b => new BookViewModel {
                    Id = b.Id,
                    CoverImagePath = b.CoverImagePath,
                    Title = b.Title,
                    Author = b.Author,
                    Likes = b.Likes,
                    Genre = b.Genre,
                    Comments = b.Comments
                }).ToList(),

                Genres = (await bookService.GetGenres()).ToList()
            };

            return View("Index", viewModel);
        }

        /// <summary>
        /// Displays detailed information about a specific book, including author information, comments, and like status.
        /// </summary>
        /// <param name="id">The unique identifier of the book to display.</param>
        /// <returns>A detailed view of the book, or NotFound if the book does not exist.</returns>
        /// <remarks>
        /// This action fetches the book with all related data including author, likes, and comments.
        /// For authenticated users, it also determines whether the current user has liked this book.
        /// ViewBag is used to pass supplementary data like AuthorBookCount and UserHasLiked to the view.
        /// </remarks>
        public async Task<IActionResult> Details(int id)
        {
            var book = await bookService.GetByIdAsync(id);

            if (book == null)
                return NotFound();

            // Retrieve the total number of books published by this author for the "About the author" section
            ViewBag.AuthorBookCount = await bookService.GetAuthorBookCount(book.AuthorId);

            // Check if the current authenticated user has already liked this book
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = userManager.GetUserId(User);
                if (userId != null)
                {
                    ViewBag.UserHasLiked = await bookService.UserHasLikedAsync(id, userId);
                }
            }

            return View(book);
        }

        /// <summary>
        /// Toggles the like status of a book for the authenticated user.
        /// If the user hasn't liked the book, this action adds a like; if already liked, it removes the like.
        /// </summary>
        /// <param name="id">The unique identifier of the book to toggle like status for.</param>
        /// <returns>A redirect back to the book details page, or Challenge if the user is not authenticated.</returns>
        /// <remarks>
        /// This action requires user authentication (enforced via [Authorize] attribute).
        /// CSRF protection is enabled via [ValidateAntiForgeryToken] to prevent unauthorized requests.
        /// The like status is toggled asynchronously and the user is redirected back to the book details page.
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
                return Challenge();

            await bookService.ToggleLikeAsync(id, userId);

            return RedirectToAction("Details", new { id });
        }
    }
}
