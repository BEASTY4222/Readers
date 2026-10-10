using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Readers.Data.DataModels;

namespace Readers.Web.ViewModels
{
    public class BookViewModel
    {
        // No need for validation because the data is from a trusted souces to an untrested source
        // (the view), and the data is already validated in the Book model.

        public int Id { get; set; }
        public string? CoverImagePath { get; set; }
        public string Title { get; set; } = null!;
        public Author Author { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
