using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Repository;
using EShop.Domain.Domain;
using System.Security.Claims;
using System.Data;
using EShop.Domain.DTO;
using EShop.Service.Interface;
using Stripe;
using EShop.Domain.Payment;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Stripe.Climate;

namespace EShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly StripeSettings _stripeSettings;

        public ShoppingCartsController(IShoppingCartService shoppingCartService, IOptions<StripeSettings> stripeSettings)
        {
            _shoppingCartService = shoppingCartService;
            _stripeSettings = stripeSettings.Value;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            return View(_shoppingCartService.getShoppingCartDetails(userId??"")); 
        }

        // GET: ShoppingCarts/Details/5
     
        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> DeleteProductFromShoppingCart(Guid? productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            var result = _shoppingCartService.deleteFromShoppingCart(userId, productId);

            return RedirectToAction("Index", "ShoppingCarts");
        }

        public Boolean Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            var result = _shoppingCartService.orderProducts(userId??"");

            return result;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var cart = _shoppingCartService.getShoppingCartDetails(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(cart.TotalPrice) * 100),
                Description = "EShop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.Order();
                return RedirectToAction("SuccessPayment");

            }
            else
            {
                return RedirectToAction("NotsuccessPayment");
            }

            return null;
        }

        public IActionResult SuccessPayment()
        {
            return View();
        }

    }
}
