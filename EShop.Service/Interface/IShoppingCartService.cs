using EShop.Domain.Domain;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IShoppingCartService
    {
        bool AddToShoppingConfirmed(BookInShoppingCart model, string userId);
        bool deleteProductFromShoppingCart(string userId, Guid productId);
        ShoppingCartDto getShoppingCartInfo(string userId);

    }
}
