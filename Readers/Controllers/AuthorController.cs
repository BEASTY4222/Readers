using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Readers.Data;
using Readers.Data.DataModels;
using Readers.Web.ViewModels.AuthorViewModels;

namespace Readers.Web.Controllers
{
    public class AuthorsController(ApplicationDbContext context) : Controller
    {

        // GET: /Authors
        public async Task<IActionResult> Index()
        {
            // 1. Fetch and map all authors
            List<AuthorListViewModel> allAuthors = await context.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.Name)
                .Select(a => new AuthorListViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Country = a.Country,
                    ImageUrl = a.ImageUrl,
                    BookCount = a.Books.Count
                })
                .ToListAsync();

            // 2. Pick the top 3 "Fan Favorites" (e.g., authors with the most books)
            List<AuthorListViewModel> featuredAuthors = allAuthors
                .OrderByDescending(a => a.BookCount)
                .Take(3)
                .ToList();

            // 3. Build the final ViewModel
            AuthorsIndexViewModel viewModel = new AuthorsIndexViewModel
            {
                FeaturedAuthors = featuredAuthors,
                AllAuthors = allAuthors
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            // Fetch the author, including their books, likes, and comments
            Author? author = await context.Authors
                .Include(a => a.Books)
                    .ThenInclude(b => b.Likes)
                .Include(a => a.Books)
                    .ThenInclude(b => b.Comments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            // Map to the ViewModel
            AuthorDetailsViewModel viewModel = new AuthorDetailsViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Country = author.Country,
                ImageUrl = author.ImageUrl,
                TotalBooks = author.Books.Count,
                Books = author.Books
                    .OrderByDescending(b => b.YearPublished)
                    .Select(b => new AuthorBookViewModel
                    {
                        Id = b.Id,
                        Title = b.Title,
                        CoverImagePath = b.CoverImagePath,
                        YearPublished = b.YearPublished,
                        LikeCount = b.Likes.Count,
                        CommentCount = b.Comments.Count
                    }).ToList()
            };

            return View(viewModel);
        }
    }
}