using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readers.Data;
using Readers.Web.ViewModels;
using Readers.Data.DataModels;
using Readers.Common;
using Readers.Core.Contracts;
using System.Linq; // for AsAsyncEnumerable() if available via package

namespace Readers.Web.Controllers
{
    public class BooksController(IBookService bookService) : Controller
    {
        [Route("/Books")]
        public async Task<IActionResult> Index()
        {
            // Retriving all the books because they arent many as if now 26.09.2026
            List<Book> books = (await bookService.GetAllBooks()).ToList();

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

        [HttpGet]
        public async Task<IActionResult> Search(SearchFormInputModel model)
        {
            var books = await bookService.SearchAsync(
                model.SearchedTitle,
                model.SearchedAuthor,
                model.SearchedGenre);

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

        public async Task<IActionResult> Details(int id)
        {
            var book = await bookService.GetByIdAsync(id);

            if (book == null)
                return NotFound();

            // Count of the author's other books (for "About the author")
            ViewBag.AuthorBookCount = await bookService.GetAuthorBookCount(book.AuthorId);

            return View(book);
        }
    }
}
