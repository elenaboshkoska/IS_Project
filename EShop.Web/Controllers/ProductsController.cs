using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Repository;
using EShop.Domain.Domain;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EShop.Domain.DTO;
using EShop.Service.Interface;

namespace EShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public ProductsController(IProductService productService, IShoppingCartService shoppingCartService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productService.GetProducts());
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind("Id,ProductName,ProductDescription,ProductImage,Price,Rating")] Product product)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                _productService.CreateNewProduct(loggedInUser, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Guid id, [Bind("Id,ProductName,ProductDescription,ProductImage,Price,Rating")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddProductToCart(Guid Id)
        {
            var result = _shoppingCartService.getProductInfo(Id);
            if(result != null)
            {
                return View(result);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddProductToCart(AddToCartDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _shoppingCartService.AddProductToShoppingCart(userId, model);

            if(result != null)
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
            else { return View(model); }
        }

        private bool ProductExists(Guid id)
        {
            return _productService.GetProductById(id) != null;
        }
    }
}
