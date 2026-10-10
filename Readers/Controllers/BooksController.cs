using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common;
using Readers.Data;
using Readers.Data.DataModels;
using Readers.ViewModels;

namespace Readers.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/Books")]
        public IActionResult Index()
        {
            // Retriving all the books because they arent many as if now 26.09.2026
            List<Book> books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .OrderBy(b => b.Author.Name)
                .ThenBy(b => b.YearPublished)
                .Take(BookControllerLimits.MaxBooksToDisplay)
                .ToList();

            List<BookViewModel> bookViewModels = books.Select(b => new BookViewModel
            {
                Id = b.Id,
                CoverImagePath = b.CoverImagePath,
                Title = b.Title,
                Author = b.Author,
                Likes = b.Likes,
                Genre = b.Genre,
                Comments = b.Comments
            }).ToList();

            return View(new BookIndexViewModel
            {
                Books = bookViewModels,
                Genres = GetGenres()
            });
        }

        [HttpGet]
        public IActionResult Search(SearchFormInputModel model)
        {
            IQueryable<Book> Books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchedTitle))
            {
                Books = Books.Where(b => b.Title.Replace(" ", String.Empty).ToLower().Contains(model.SearchedTitle.Replace(" ", String.Empty).ToLower()));
            }

            if (!string.IsNullOrEmpty(model.SearchedAuthor))
            {
                Books = Books.Where(b => b.Author.Name.Replace(" ", String.Empty).ToLower().Contains(model.SearchedAuthor.Replace(" ", String.Empty).ToLower()));
            }

            if(!string.IsNullOrEmpty(model.SearchedGenre))
            {
                Books = Books.Where(b => b.Genre == model.SearchedGenre);
            }

            Books = Books.Take(BookControllerLimits.MaxBooksToDisplay);

            List<BookViewModel> bookViewModels = Books.Select(b => new BookViewModel
            {
                Id = b.Id,
                CoverImagePath = b.CoverImagePath,
                Title = b.Title,
                Author = b.Author,
                Likes = b.Likes,
                Comments = b.Comments
            })
            .OrderBy(b => b.Author.Name)
            .ToList();

            return View("Index", new BookIndexViewModel
            {
                Books = bookViewModels,
                Genres = GetGenres()
            });
        }

        private List<string> GetGenres()
        {
            return _context.Books
                .Select(book => book.Genre)
                .Distinct()
                .OrderBy(genre => genre)
                .ToList();
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            // Count of the author's other books (for "About the author")
            ViewBag.AuthorBookCount =  _context.Books
                .Count(b => b.AuthorId == book.AuthorId);

            return View(book);
        }
    }
}
