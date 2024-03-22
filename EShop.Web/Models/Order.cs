using System.ComponentModel.DataAnnotations;

namespace EShop.Web.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string? OwnerId { get; set; }
        public EShopApplicationUser? Owner { get; set; }

        public ICollection<ProductInOrder>? ProductInOrders { get; set; }
    }
}
