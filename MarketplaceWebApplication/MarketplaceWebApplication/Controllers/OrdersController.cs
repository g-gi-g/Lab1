using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Extensions;
using MarketplaceWebApplication.Models;

namespace MarketplaceWebApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DbmarketplaceContext _context;

        public OrdersController(DbmarketplaceContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            int userId = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;
            ViewData["UserId"] = userId;

            var dbmarketplaceContext = _context.Orders.Include(o => o.Customer)
                .Include(o => o.PaymentMethod)
                .Include(o => o.Status)
                .Where(o => o.CustomerId == userId);

            ViewData["PaymentMethods"] = _context.PaymentMethods.ToList();
            ViewData["OrderStatuses"] = _context.OrderStatuses.ToList();

            return View(await dbmarketplaceContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PaymentMethod)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id");
            ViewData["PaymentMethodName"] = new SelectList(_context.PaymentMethods, "Id", "Name");

            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            ViewData["UserId"] = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,TransactionId,StatusId,DateOfOrder,PaymentMethodId,Comment")] OrderModel order)
        {
            order.DateOfOrder = DateTime.Now;
            if (ModelState.IsValid)
            {
                Order or = new Order 
                {
                    CustomerId = order.CustomerId,
                    TransactionId = order.TransactionId,
                    StatusId = order.StatusId,
                    DateOfOrder = order.DateOfOrder,
                    PaymentMethodId = order.PaymentMethodId,
                    Comment = order.Comment,
                };
                _context.Add(or);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["PaymentMethodName"] = new SelectList(_context.PaymentMethods, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            ViewData["UserId"] = order.CustomerId;
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", order.PaymentMethodId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,TransactionId,StatusId,DateOfOrder,PaymentMethodId,Comment")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", order.PaymentMethodId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.StatusId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PaymentMethod)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
