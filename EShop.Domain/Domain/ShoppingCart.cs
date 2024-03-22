using EShop.Domain.Identity;
using System.ComponentModel.DataAnnotations;


namespace EShop.Domain.Domain
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        public string? OwnerId { get; set; }
        public EShopApplicationUser? Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart>? ProductInShoppingCarts { get; set; }
    }
}
