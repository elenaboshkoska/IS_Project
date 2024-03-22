namespace EShop.Web.Models.DTO
{
    public class ShoppingCartDTO
    {
        public List<ProductInShoppingCart>? AllProducts { get; set; }
        public double TotalPrice { get; set; }
    }
}
