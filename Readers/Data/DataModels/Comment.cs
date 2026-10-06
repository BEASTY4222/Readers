using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Readers.Data.DataModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [MinLength(EntityDataLimits.CommentContentMinLenght)]
        [MaxLength(EntityDataLimits.CommentContentMaxLength)]
        public string Content { get; set; } = null!;

        [Column(TypeName = EntityDataLimits.collumnType)]
        public DateTime CreatedAt { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
