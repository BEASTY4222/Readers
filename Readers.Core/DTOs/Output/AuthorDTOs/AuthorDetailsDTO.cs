using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.DTOs.Output.AuthorDTOs
{
    public class AuthorDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public string? ImageUrl { get; set; }
        public int TotalBooks { get; set; }

        public List<AuthorBookDTO> Books { get; set; } = new();
    }
}
