using System.ComponentModel.DataAnnotations;

namespace WishlistAPI.Models
{
    public class Rating: BaseEntity
    {
        [Required]
        [RegularExpression(@"^\d+\.\d{0,1}$")]
        [Range(0, 5)]
        public short? Rate { get; set; }

        [Required]
        public int Count { get; set; } = 0;
    }
}
