using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WishlistAPI.Models
{
    public class Product : BaseEntity
    {
        [Required, StringLength(100)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Price { get; set; }

        public string Picture { get; set; } = string.Empty;

        [ForeignKey("Rating")]
        public int? RatingId { get; set; }
        public Rating? Rating { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}