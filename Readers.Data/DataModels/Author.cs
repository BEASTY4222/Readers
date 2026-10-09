using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Readers.Web.common;

namespace Readers.Web.Data.DataModels
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(EntityDataLimits.NameMinLenght, EntityDataLimits.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Range(EntityDataLimits.CountryMinLenght, EntityDataLimits.CountryMaxLength)]
        public string? Country { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
