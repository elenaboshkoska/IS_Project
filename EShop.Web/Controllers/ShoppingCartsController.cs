using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Repository;
using EShop.Domain.Domain;
using System.Security.Claims;
using System.Data;
using EShop.Domain.DTO;

namespace EShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            if (userId != null)
            {
                var loggedInUser = await _context.Users
                    .Include(z => z.UserCart)
                    .Include("UserCart.ProductInShoppingCarts")
                    .Include("UserCart.ProductInShoppingCarts.Product")
                    .FirstOrDefaultAsync(z => z.Id == userId);

                var allProducts = loggedInUser?.UserCart.ProductInShoppingCarts.ToList();

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

                return View(model);

            }

            return View();  
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                shoppingCart.Id = Guid.NewGuid();
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,OwnerId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> DeleteProductFromShoppingCart(Guid? productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            if (userId != null)
            {
                var loggedInUser = await _context.Users
                    .Include(z => z.UserCart)
                    .Include("UserCart.ProductInShoppingCarts")
                    .Include("UserCart.ProductInShoppingCarts.Product")
                    .FirstOrDefaultAsync(z => z.Id == userId);


                var product_to_delete = loggedInUser?.UserCart.ProductInShoppingCarts.First(z => z.ProductId == productId);

                loggedInUser?.UserCart.ProductInShoppingCarts.Remove(product_to_delete);

                _context.ShoppingCarts.Update(loggedInUser?.UserCart);
                _context.SaveChanges();

                return RedirectToAction("Index", "ShoppingCarts");

            }
            return RedirectToAction("Index", "ShoppingCarts");
        }

        public async Task<IActionResult> Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            if (userId != null)
            {
                var loggedInUser = await _context.Users
                        .Include(z => z.UserCart)
                        .Include("UserCart.ProductInShoppingCarts")
                        .Include("UserCart.ProductInShoppingCarts.Product")
                        .FirstOrDefaultAsync(z => z.Id == userId);

                var userCart = loggedInUser?.UserCart;

                var userOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    OwnerId = userId,
                    Owner = loggedInUser
                };

                _context.Orders.Add(userOrder);
                _context.SaveChanges();

                var productInOrders = userCart?.ProductInShoppingCarts.Select(z => new ProductInOrder
                {
                    Order = userOrder,
                    OrderId = userOrder.Id,
                    ProductId = z.ProductId,
                    OrderedProduct = z.Product,
                    Quantity = z.Quantity
                }).ToList();

                _context.ProductInOrders.AddRange(productInOrders);
                _context.SaveChanges();

                userCart?.ProductInShoppingCarts.Clear();

                _context.ShoppingCarts.Update(userCart);
                _context.SaveChanges();

                return RedirectToAction("Index", "ShoppingCarts");
            }
            return RedirectToAction("Index", "ShoppingCarts");
        }



        private bool ShoppingCartExists(Guid id)
        {
            return _context.ShoppingCarts.Any(e => e.Id == id);
        }
    }
}
