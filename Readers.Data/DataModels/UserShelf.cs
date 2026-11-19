using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Readers.Data.DataModels
{
    public class UserShelf
    {
        [Key]
        public Guid Id { get; set; }
        
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        public ShelfStatus status { get; set; }

        public DateTime DateAdded { get; set; }

    }
}
