using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Domain;
using EShop.Repository;
using EShop.Service.Interface;
using System.Security.Claims;
using EShop.Service.Implementation;

namespace EShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
     
        private readonly IShoppingCartService shoppingCartService;
        public ShoppingCartsController(IShoppingCartService shoppingCartServicet)
        {
            this.shoppingCartService = shoppingCartServicet;
        }

        // GET: ShoppingCarts
        public  IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = shoppingCartService.getShoppingCartInfo(userId);
            return View(dto);
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            shoppingCartService.deleteProductFromShoppingCart(userId, id);

            return RedirectToAction("Index");

        }

    }
}
