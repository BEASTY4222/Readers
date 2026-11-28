using Readers.Core.DTOs.Output.AuthorDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Contracts
{
    public interface IAuthorService
    {
        Task<List<AuthorListDTO>> GetAllAuthorsAsync();
        Task<List<AuthorListDTO>> GetFeaturedAuthorsAsync();
        Task<AuthorDetailsDTO?> GetAuthorDetailsAsync(Guid authorId);
    }
}
