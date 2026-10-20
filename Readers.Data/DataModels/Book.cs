using System.ComponentModel.DataAnnotations;
using Readers.Common;

namespace Readers.Data.DataModels
{
    /// <summary>
    /// Represents a book entity in the database.
    /// Contains core book information including title, author reference, genre, publication details,
    /// and relationships to user interactions (likes and comments).
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        /// <remarks>
        /// Required field. Length must be between TitleMinLength and TitleMaxLength characters.
        /// </remarks>
        [Required]
        [StringLength(EntityDataLimits.TitleMaxLength, MinimumLength = EntityDataLimits.TitleMinLenght)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the file path or URL to the book's cover image.
        /// </summary>
        /// <remarks>
        /// Optional field. May be null if no cover image has been uploaded.
        /// </remarks>
        public string? CoverImagePath { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the book's author.
        /// </summary>
        /// <remarks>
        /// Required foreign key referencing the Author entity.
        /// </remarks>
        [Required]
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the book's author.
        /// </summary>
        /// <remarks>
        /// Populated via Entity Framework navigation. Always loaded with Include(b => b.Author) queries.
        /// </remarks>
        public Author Author { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the publishing house that published this book.
        /// </summary>
        /// <remarks>
        /// Required field. Length must be between PublishingHouseMinLength and PublishingHouseMaxLength characters.
        /// </remarks>
        [Required]
        [StringLength(EntityDataLimits.PublishingHouseMaxLength, MinimumLength = EntityDataLimits.PublishingHouseMinLenght)]
        public string PublishingHouse { get; set; } = null!;

        /// <summary>
        /// Gets or sets the genre/category of the book (e.g., "Fiction", "Science", "Biography").
        /// </summary>
        /// <remarks>
        /// Required field. Length must be between GenreMinLength and GenreMaxLength characters.
        /// Used for filtering and categorization in the user interface.
        /// </remarks>
        [Required]
        [StringLength(EntityDataLimits.GenreMaxLength, MinimumLength = EntityDataLimits.GenreMinLenght)]
        public string Genre { get; set; } = null!;

        /// <summary>
        /// Gets or sets the year the book was first published.
        /// </summary>
        /// <remarks>
        /// Required field. Must be within YearPublishedMinLength and YearPublishedMaxLength range.
        /// </remarks>
        [Required]
        [Range(EntityDataLimits.YearPublishedMinLenght, EntityDataLimits.YearPublishedMaxLength)]
        public int YearPublished { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of pages in the book.
        /// </summary>
        /// <remarks>
        /// Required field. Must be between PagesMinLength and PagesMaxLength.
        /// </remarks>
        [Required]
        [Range(EntityDataLimits.PagesMinLenght, EntityDataLimits.PagesMaxLength)]
        public int Pages { get; set; } = 0;

        /// <summary>
        /// Gets or sets the collection of likes this book has received from users.
        /// </summary>
        /// <remarks>
        /// Virtual collection for lazy loading or eager loading with Include().
        /// Initialized to an empty list by default.
        /// </remarks>
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

        /// <summary>
        /// Gets or sets the collection of comments users have posted on this book.
        /// </summary>
        /// <remarks>
        /// Virtual collection for lazy loading or eager loading with Include().
        /// Initialized to an empty list by default.
        /// </remarks>
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}