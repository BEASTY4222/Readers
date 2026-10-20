using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Readers.Common;

namespace Readers.Data.DataModels
{
    /// <summary>
    /// Represents an author entity in the database.
    /// Contains author biographical information and maintains a relationship to all books published by this author.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets the unique identifier for the author.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <remarks>
        /// Required field. Length must be between NameMinLength and NameMaxLength characters.
        /// </remarks>
        [Required]
        [Range(EntityDataLimits.NameMinLenght, EntityDataLimits.NameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the country of origin or residence of the author.
        /// </summary>
        /// <remarks>
        /// Optional field. Length must be between CountryMinLength and CountryMaxLength characters if provided.
        /// May be null if author country is unknown or not provided.
        /// </remarks>
        [Range(EntityDataLimits.CountryMinLenght, EntityDataLimits.CountryMaxLength)]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the collection of books published by this author.
        /// </summary>
        /// <remarks>
        /// Virtual collection for lazy loading or eager loading with Include().
        /// Initialized to an empty list by default.
        /// </remarks>
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
