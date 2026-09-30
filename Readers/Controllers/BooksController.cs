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
                CoverImagePath = b.CoverImagePath,
                Title = b.Title,
                Author = b.Author,
                Likes = b.Likes,
                Comments = b.Comments
            }).ToList();

            return View(bookViewModels);
        }

        //[HttpGet]
        public IActionResult Index([FromQuery] string searchedBook)
        {
            // Retriving all the books because they arent many as if now 26.09.2026
            List<Book> books = _context.Books
                // big string formatting to remove whitespace and make it easier to search for books with spaces in their titles
                .Where(b => b.Title.Replace(" ", String.Empty).ToLower().Contains(searchedBook.Replace(" ", String.Empty).ToLower()))
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .OrderBy(b => b.Author.Name)
                .ThenBy(b => b.YearPublished)
                .Take(BookControllerLimits.MaxBooksToDisplay)
                .ToList();

            List<BookViewModel> bookViewModels = books.Select(b => new BookViewModel
            {
                CoverImagePath = b.CoverImagePath,
                Title = b.Title,
                Author = b.Author,
                Likes = b.Likes,
                Comments = b.Comments
            }).ToList();

            return View(bookViewModels);
        }
    }
}
