using EShop.Domain.Domain;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<Product> productRepository, IRepository<Order> orderRepository, IRepository<ProductInOrder> productInOrderRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _emailService = emailService;
        }

        public ShoppingCart AddProductToShoppingCart(string userId, AddToCartDTO model)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userCart = loggedInUser?.UserCart;

                var selectedProduct = _productRepository.Get(model.SelectedProductId);

                if (selectedProduct != null && userCart != null)
                {
                    userCart?.ProductInShoppingCarts?.Add(new ProductInShoppingCart
                    {
                        Product = selectedProduct,
                        ProductId = selectedProduct.Id,
                        ShoppingCart = userCart,
                        ShoppingCartId = userCart.Id,
                        Quantity = model.Quantity
                    });

                    return _shoppingCartRepository.Update(userCart);
                }
            }
            return null;
        }

        public bool deleteFromShoppingCart(string userId, Guid? Id)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);


                var product_to_delete = loggedInUser?.UserCart?.ProductInShoppingCarts.First(z => z.ProductId == Id);

                loggedInUser?.UserCart?.ProductInShoppingCarts?.Remove(product_to_delete);

                _shoppingCartRepository.Update(loggedInUser.UserCart);

                return true;

            }

            return false;
        }

        public AddToCartDTO getProductInfo(Guid Id)
        {
            var selectedProduct = _productRepository.Get(Id);
            if (selectedProduct != null)
            {
                var model = new AddToCartDTO
                {
                    SelectedProductName = selectedProduct.ProductName,
                    SelectedProductId = selectedProduct.Id,
                    Quantity = 1
                };
                return model;
            }
            return null;
        }

        public ShoppingCartDTO getShoppingCartDetails(string userId)
        {
            if (userId != null && !userId.IsNullOrEmpty())
            {
                var loggedInUser = _userRepository.Get(userId);

                var allProducts = loggedInUser?.UserCart?.ProductInShoppingCarts?.ToList();

                var totalPrice = 0.0;

                foreach (var item in allProducts)
                {
                    totalPrice += Double.Round((item.Quantity * item.Product.Price), 2);
                }

                var model = new ShoppingCartDTO
                {
                    AllProducts = allProducts,
                    TotalPrice = totalPrice
                };

                return model;

            }

            return new ShoppingCartDTO
            {
                AllProducts = new List<ProductInShoppingCart>(),
                TotalPrice = 0.0
            };
        }

        public bool orderProducts(string userId)
        {
            if (userId != null && !userId.IsNullOrEmpty())
            {
                var loggedInUser = _userRepository.Get(userId);

                var userCart = loggedInUser?.UserCart;

                EmailMessage message = new EmailMessage();
                message.Subject = "Successfull order";
                message.MailTo = loggedInUser.Email;

                var userOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    OwnerId = userId,
                    Owner = loggedInUser
                };

                _orderRepository.Insert(userOrder);

                var productInOrders = userCart?.ProductInShoppingCarts.Select(z => new ProductInOrder
                {
                    Order = userOrder,
                    OrderId = userOrder.Id,
                    ProductId = z.ProductId,
                    OrderedProduct = z.Product,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= productInOrders.Count(); i++)
                {
                    var currentItem = productInOrders[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.OrderedProduct.Price;
                    sb.AppendLine(i.ToString() + ". " + currentItem.OrderedProduct.ProductName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.OrderedProduct.Price);
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                message.Content = sb.ToString();

                _productInOrderRepository.InsertMany(productInOrders);

                userCart?.ProductInShoppingCarts.Clear();

                _shoppingCartRepository.Update(userCart);

                this._emailService.SendEmailAsync(message);

                return true;
            }
            return false;
        }
    }
}
