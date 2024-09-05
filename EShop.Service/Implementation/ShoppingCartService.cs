using EShop.Domain.Domain;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> shoppingCartRepository;
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<BookInShoppingCart> _bookInShoppingCartRepository, IUserRepository _userRepository) 
        {
            this._bookInShoppingCartRepository = _bookInShoppingCartRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this._userRepository = _userRepository;
        }
        public bool AddToShoppingConfirmed(BookInShoppingCart model, string userId)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userShoppingCart = loggedInUser.ShoppingCart;
            if (userShoppingCart.BookInShoppingCarts == null)
            {
                userShoppingCart.BookInShoppingCarts = new List<BookInShoppingCart>();
            }
            userShoppingCart.BookInShoppingCarts.Add(model);
            shoppingCartRepository.Update(userShoppingCart);
            return true;
         }
        public bool deleteProductFromShoppingCart(string userId, Guid productId)
        {
            if (productId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                var product = userShoppingCart.BookInShoppingCarts.Where(x => x.BookId == productId).FirstOrDefault();

                userShoppingCart.BookInShoppingCarts.Remove(product);

                shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;

        }
        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser?.ShoppingCart;
            var allBooks = userShoppingCart?.BookInShoppingCarts?.ToList();

            var totalPrice = allBooks.Select(x => (x.Book.Price * x.Quantity)).Sum();

            ShoppingCartDto dto = new ShoppingCartDto
            {
                Books = allBooks,
                TotalPrice = totalPrice
            };
            return dto;
        }
    }
}
