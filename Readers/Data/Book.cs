using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Common;

namespace Readers.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(EntityDataLimits.TitleMaxLength, MinimumLength = EntityDataLimits.TitleMinLenght)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(EntityDataLimits.AuthorMaxLength, MinimumLength = EntityDataLimits.AuthorMinLenght)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(EntityDataLimits.PublishingHouseMaxLength, MinimumLength = EntityDataLimits.PublishingHouseMinLenght)]
        public string PublishingHouse { get; set; } = null!;

        [Required]
        [StringLength(EntityDataLimits.GanreMaxLength, MinimumLength = EntityDataLimits.GanreMinLenght)]
        public string Ganre { get; set; } = null!;

        [Required]
        [Range(EntityDataLimits.YearPublishedMinLenght, EntityDataLimits.YearPublishedMaxLength)]
        public int YearPublished { get; set; } = 0;

        [Required]
        [Range(EntityDataLimits.PagesMinLenght, EntityDataLimits.PagesMaxLength)]
        public int pages { get; set; } = 0;

        public virtual int Comments { get; set; } = 0;

        public virtual int likes { get; set; } = 0;
    }
}