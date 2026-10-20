using System.ComponentModel.DataAnnotations;

namespace Readers.Data.DataModels
{
    /// <summary>
    /// Represents a user's "like" action on a book.
    /// This entity creates a many-to-many relationship between users and books,
    /// allowing users to mark books as liked and track the total like count per book.
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Gets or sets the unique identifier for this like record.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the book being liked.
        /// </summary>
        /// <remarks>
        /// Foreign key referencing the Book entity.
        /// </remarks>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the book being liked.
        /// </summary>
        public Book book { get; set; } = null!;


        /// <summary>
        /// Gets or sets the unique identifier of the user who liked the book.
        /// </summary>
        /// <remarks>
        /// Foreign key referencing the ApplicationUser entity.
        /// </remarks>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation property to the user who liked the book.
        /// </summary>
        public ApplicationUser user { get; set; } = null!;
    }
}
