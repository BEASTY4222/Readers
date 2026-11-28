using Microsoft.EntityFrameworkCore;
using Readers.Core.Contracts;
using Readers.Core.DTOs.Output.AuthorDTOs;
using Readers.Data;
using Readers.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Services
{
    public class AuthorService(ApplicationDbContext context) : IAuthorService
    {
        public async Task<List<AuthorListDTO>> GetAllAuthorsAsync()
        {
            return await context.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.Name)
                .Select(a => new AuthorListDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Country = a.Country,
                    ImageUrl = a.ImageUrl,
                    BookCount = a.Books.Count
                })
                .ToListAsync();
        }

        public async Task<List<AuthorListDTO>> GetFeaturedAuthorsAsync()
        {
            List<AuthorListDTO> authors = await GetAllAuthorsAsync();
            return authors
                .OrderByDescending(a => a.BookCount)
                .Take(3)
                .ToList();
        }

        public async Task<AuthorDetailsDTO?> GetAuthorDetailsAsync(Guid authorId)
        {
            Author? author = await context.Authors
                .Include(a => a.Books)
                    .ThenInclude(b => b.Likes)
                .Include(a => a.Books)
                    .ThenInclude(b => b.Comments)
                .FirstOrDefaultAsync(a => a.Id == authorId);

            if (author is null)
            {
                return null;
            }

            return new AuthorDetailsDTO
            {
                Id = author.Id,
                Name = author.Name,
                Country = author.Country,
                ImageUrl = author.ImageUrl,
                TotalBooks = author.Books.Count,
                Books = author.Books
                    .OrderByDescending(b => b.YearPublished)
                    .Select(b => new AuthorBookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        CoverImagePath = b.CoverImagePath,
                        YearPublished = b.YearPublished,
                        LikeCount = b.Likes.Count,
                        CommentCount = b.Comments.Count
                    }).ToList()
            };
        }
    }
}
