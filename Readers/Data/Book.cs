using System.ComponentModel.DataAnnotations;

namespace Readers.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public string PublishingHouse { get; set; } = null!;

        [Required]
        public string Ganre { get; set; } = null!;

        [Required]
        public int? YearPublished { get; set; } = null!;

        [Required]
        public int? pages { get; set; } = null!;

        public virtual int Comments { get; set; } = 0;

        public virtual int likes { get; set; } = 0;
    }
}