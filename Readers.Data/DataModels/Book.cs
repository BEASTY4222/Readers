using System.ComponentModel.DataAnnotations;
using Readers.Web.common;

namespace Readers.Web.Data.DataModels
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(EntityDataLimits.TitleMaxLength, MinimumLength = EntityDataLimits.TitleMinLenght)]
        public string Title { get; set; } = null!;

        public string? CoverImagePath { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author Author { get; set; } = null!;

        [Required]
        [StringLength(EntityDataLimits.PublishingHouseMaxLength, MinimumLength = EntityDataLimits.PublishingHouseMinLenght)]
        public string PublishingHouse { get; set; } = null!;

        [Required]
        [StringLength(EntityDataLimits.GenreMaxLength, MinimumLength = EntityDataLimits.GenreMinLenght)]
        public string Genre { get; set; } = null!;

        [Required]
        [Range(EntityDataLimits.YearPublishedMinLenght, EntityDataLimits.YearPublishedMaxLength)]
        public int YearPublished { get; set; } = 0;

        [Required]
        [Range(EntityDataLimits.PagesMinLenght, EntityDataLimits.PagesMaxLength)]
        public int Pages { get; set; } = 0;

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}