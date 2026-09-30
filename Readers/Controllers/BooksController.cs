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

        [HttpGet]
        public IActionResult Search(SearchFormViewModel model)
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
                Books = Books.Where(b => b.Ganre == model.SearchedGenre);
            }

            Books = Books.Take(BookControllerLimits.MaxBooksToDisplay);

            List<BookViewModel> bookViewModels = Books.Select(b => new BookViewModel
            {
                CoverImagePath = b.CoverImagePath,
                Title = b.Title,
                Author = b.Author,
                Likes = b.Likes,
                Comments = b.Comments
            })
            .OrderBy(b => b.Author.Name)
            .ToList();

            return View("Index",bookViewModels);
        }
    }
}
