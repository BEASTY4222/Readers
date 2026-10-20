using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Readers.Common;

namespace Readers.Data.DataModels
{
    /// <summary>
    /// Represents a user comment on a book.
    /// Stores the comment content, creation timestamp, and relationships to the book and user who posted it.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the comment.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text content of the comment.
        /// </summary>
        /// <remarks>
        /// Length must be between CommentContentMinLength and CommentContentMaxLength characters (5-500 chars).
        /// </remarks>
        [MinLength(EntityDataLimits.CommentContentMinLenght)]
        [MaxLength(EntityDataLimits.CommentContentMaxLength)]
        public string Content { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the comment was created (UTC).
        /// </summary>
        /// <remarks>
        /// Stored as datetime2 column type in the database.
        /// Set to DateTime.UtcNow when the comment is first created.
        /// </remarks>
        [Column(TypeName = EntityDataLimits.collumnType)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the book this comment is posted on.
        /// </summary>
        /// <remarks>
        /// Foreign key referencing the Book entity.
        /// </remarks>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the book this comment belongs to.
        /// </summary>
        public Book Book { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the user who posted this comment.
        /// </summary>
        /// <remarks>
        /// Foreign key referencing the ApplicationUser entity.
        /// </remarks>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation property to the user who posted this comment.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
    }
}
