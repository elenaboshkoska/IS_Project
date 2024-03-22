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

namespace EShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,ProductName,ProductDescription,ProductImage,Price,Rating")] Product product)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var createdBy = _context.Users
                    .Where(u => u.Id == loggedInUser)
                    .FirstOrDefault();
                product.Id = Guid.NewGuid();
                product.CreatedBy = createdBy;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductName,ProductDescription,ProductImage,Price,Rating")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddProductToCart(Guid Id)
        {
            var selectedProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == Id);
            if(selectedProduct != null)
            {
                var model = new AddToCartDTO
                {
                    SelectedProductName = selectedProduct.ProductName,
                    SelectedProductId = selectedProduct.Id,
                    Quantity = 1
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(AddToCartDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)??null;
            
            if (userId != null) {
                var loggedInUser = await _context.Users
                    .Include(z => z.UserCart)
                    .Include("UserCart.ProductInShoppingCarts")
                    .FirstOrDefaultAsync(z => z.Id == userId);

                var userCart = loggedInUser?.UserCart;

                var selectedProduct = await _context.Products.FirstOrDefaultAsync(z => z.Id == model.SelectedProductId);

                if(selectedProduct != null && userCart != null)
                {
                    userCart.ProductInShoppingCarts.Add(new ProductInShoppingCart
                    {
                        Product = selectedProduct,
                        ProductId = selectedProduct.Id,
                        ShoppingCart = userCart,
                        ShoppingCartId = userCart.Id,
                        Quantity = model.Quantity
                    });

                    _context.Update(userCart);

                    _context.SaveChanges();

                    return RedirectToAction("Index", "ShoppingCarts");
                }

            }

            return View(model);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
