using System.ComponentModel.DataAnnotations;

namespace WishlistAPI.Models
{
    public class Category: BaseEntity
    {
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public readonly ICollection<Product> Products = new List<Product>();
    }
}
