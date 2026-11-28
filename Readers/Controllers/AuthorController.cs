using Microsoft.AspNetCore.Mvc;
using Readers.Web.ViewModels.AuthorViewModels;
using Readers.Core.Contracts;
using Readers.Core.DTOs.Output.AuthorDTOs;

namespace Readers.Web.Controllers
{
    public class AuthorsController(IAuthorService authorService) : Controller
    {

        // GET: /Authors
        public async Task<IActionResult> Index()
        {
            // 1. Fetch and map all authors
            List<AuthorListViewModel> allAuthors = (await authorService.GetAllAuthorsAsync())
                .Select(author => new AuthorListViewModel
                {
                    Id = author.Id,
                    Name = author.Name,
                    Country = author.Country,
                    ImageUrl = author.ImageUrl,
                    BookCount = author.BookCount
                })
                .ToList();

            // 2. Pick the top 3 "Fan Favorites" (e.g., authors with the most books)
            List<AuthorListViewModel> featuredAuthors = (await authorService.GetFeaturedAuthorsAsync())
                .Select(author => new AuthorListViewModel
                {
                    Id = author.Id,
                    Name = author.Name,
                    Country = author.Country,
                    ImageUrl = author.ImageUrl,
                    BookCount = author.BookCount
                })
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
            AuthorDetailsDTO? author = await authorService.GetAuthorDetailsAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            AuthorDetailsViewModel viewModel = new AuthorDetailsViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Country = author.Country,
                ImageUrl = author.ImageUrl,
                TotalBooks = author.TotalBooks,
                Books = author.Books
                    .Select(book => new AuthorBookViewModel
                    {
                        Id = book.Id,
                        Title = book.Title,
                        CoverImagePath = book.CoverImagePath,
                        YearPublished = book.YearPublished,
                        LikeCount = book.LikeCount,
                        CommentCount = book.CommentCount
                    }).ToList()
            };

            return View(viewModel);
        }
    }
}