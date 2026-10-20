using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Readers.Data.DataModels;

namespace Readers.Web.ViewModels
{
    /// <summary>
    /// Represents a book for display in the view layer.
    /// Maps essential book data from the domain Book entity for safe presentation in Razor views.
    /// No validation attributes are needed since data comes from a trusted internal source (database)
    /// to an untrusted external sink (HTML view), and the data is already validated in the Book model.
    /// </summary>
    public class BookViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the file path or URL to the book's cover image.
        /// </summary>
        /// <remarks>
        /// May be null if no cover image has been uploaded.
        /// </remarks>
        public string? CoverImagePath { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        /// <remarks>
        /// Contains the Author entity with the author's name and publication details.
        /// </remarks>
        public Author Author { get; set; } = null!;

        /// <summary>
        /// Gets or sets the genre/category of the book.
        /// </summary>
        public string Genre { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of likes this book has received.
        /// </summary>
        /// <remarks>
        /// Used to display the like count in the view.
        /// </remarks>
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

        /// <summary>
        /// Gets or sets the collection of comments posted on this book.
        /// </summary>
        /// <remarks>
        /// Used to display user comments in the book details view.
        /// </remarks>
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
