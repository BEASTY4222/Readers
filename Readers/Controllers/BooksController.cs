using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readers.Data;

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
        public async Task<IActionResult> Index()
        {
            // Retriving all the books because they arent many as if now 26.09.2026
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Likes)
                .Include(b => b.Comments)
                .OrderBy(b => b.Author.Name)
                .ThenBy(b => b.YearPublished)
                .ToListAsync();

            return View(books);
        }
    }
}
