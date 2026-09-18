using System.ComponentModel.DataAnnotations;

namespace Readers.Data.DataModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
    
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
