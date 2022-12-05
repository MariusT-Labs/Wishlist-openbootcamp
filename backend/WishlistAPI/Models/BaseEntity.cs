using System.ComponentModel.DataAnnotations;

namespace WishlistAPI.Models
{
    public class BaseEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
