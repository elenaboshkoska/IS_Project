using EShop.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Domain
{
    public class Product : BaseEntity
    {
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        [Required]
        public string? ProductImage { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Rating { get; set; }
        public virtual EShopApplicationUser? CreatedBy { get; set; }
        public virtual ICollection<ProductInShoppingCart>? ProductsInShoppingCart { get; set; }
        public ICollection<ProductInOrder>? ProductInOrders { get; set; }
    }
}
