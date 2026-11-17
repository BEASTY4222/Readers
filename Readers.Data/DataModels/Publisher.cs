using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Readers.Common;

namespace Readers.Data.DataModels
{
    public class Publisher
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(EntityDataLimits.PublisherNameMaxLength)]
        [MinLength(EntityDataLimits.PublisherNameMinLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
