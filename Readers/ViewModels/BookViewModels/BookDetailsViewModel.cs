using Readers.Common;
using Readers.Data.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Readers.Web.ViewModels.BookViewModels
{
    public class BookDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? CoverImagePath { get; set; }

        public Author Author { get; set; } = null!;
        public ShelfStatus? ShelfStatus { get; set; }

        public string PublisherName { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public int YearPublished { get; set; } = 0;

        public int Pages { get; set; } = 0;

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

