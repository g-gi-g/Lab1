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
    public class OrderItemsController : Controller
    {
        private readonly DbmarketplaceContext _context;

        public OrderItemsController(DbmarketplaceContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            int userId = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;

            var dbmarketplaceContext = _context.OrderItems.Include(o => o.Offer).Include(o => o.Order).Where(o => o.Order.CustomerId == userId);
            return View(await dbmarketplaceContext.ToListAsync());
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Offer)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create(int Id)
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            int userId = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;

            var offer = _context.Offers.FirstOrDefault(o => o.Id == Id);
            ViewData["UserId"] = userId;
            ViewData["Offer"] = offer;
            ViewData["OfferId"] = new SelectList(new List<Offer> { offer }, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id");
            ViewData["PaymentMethodName"] = new SelectList(_context.PaymentMethods, "Id", "Name");

            return View();
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OfferId,Price,Quantity,CustomerId,TransactionId,StatusId,DateOfOrder,PaymentMethodId,Comment")] OrderItemForm orderItemForm)
        {
            orderItemForm.DateOfOrder = DateTime.Now;

            if (ModelState.IsValid)
            {
                Order order = new Order 
                {
                    CustomerId = orderItemForm.CustomerId,
                    TransactionId = orderItemForm.TransactionId,
                    StatusId = orderItemForm.StatusId,
                    DateOfOrder = orderItemForm.DateOfOrder,
                    PaymentMethodId = orderItemForm.PaymentMethodId,
                    Comment = orderItemForm.Comment,
                };
                _context.Add(order);
                await _context.SaveChangesAsync();

                int orderId = order.Id;

                OrderItem oi = new OrderItem
                {
                    OrderId = orderId,
                    OfferId = orderItemForm.OfferId,
                    Price = orderItemForm.Price,
                    Quantity = orderItemForm.Quantity,
                };
                _context.Add(oi);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            var offer = _context.Offers.FirstOrDefault(o => o.Id == orderItemForm.OfferId);
            ViewData["UserId"] = orderItemForm.CustomerId;
            ViewData["Offer"] = offer;
            ViewData["PaymentMethods"] = _context.PaymentMethods.ToList();
            ViewData["OrderStatuses"] = _context.OrderStatuses.ToList();
            ViewData["OfferId"] = new SelectList(new List<Offer> { offer }, "Id", "Name");
            return View(orderItemForm);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", orderItem.OfferId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,OfferId,Price,Quantity")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", orderItem.OfferId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Offer)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
