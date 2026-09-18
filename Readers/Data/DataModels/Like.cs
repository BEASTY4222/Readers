using System.ComponentModel.DataAnnotations;

namespace Readers.Data.DataModels
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book book { get; set; } = null!;


        public string UserId { get; set; } = null!;
        public ApplicationUser user { get; set; } = null!;
    }
}
