using System.ComponentModel.DataAnnotations;


namespace EShop.Domain.Domain
{
    public class ProductInOrder : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product? OrderedProduct { get; set; }

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        public int Quantity { get; set; }
    }
}
