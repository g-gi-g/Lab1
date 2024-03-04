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

        public async Task<IActionResult> Clients()
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            int userId = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;

            var dbmarketplaceContext = _context.OrderItems.Include(o => o.Offer).Include(o => o.Order).Where(o => o.Offer.SellerId == userId);
            return View(await dbmarketplaceContext.ToListAsync());
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            int userId = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;

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

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderItem.OrderId);
            var shipping = await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderItem.OrderId);
            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == orderItem.OfferId);
            ViewData["Offer"] = offer;
            ViewData["Order"] = order;
            ViewData["Shipping"] = shipping;

            ViewData["UserId"] = userId;
            ViewData["Status"] = await _context.OrderStatuses.FirstOrDefaultAsync(os => os.Id == order.StatusId);
            ViewData["PaymentMethod"] = await _context.PaymentMethods.FirstOrDefaultAsync(p => p.Id == order.PaymentMethodId);
            ViewData["ShippingCompany"] = await _context.ShippingCompanies.FirstOrDefaultAsync(s => s.Id == shipping.ShippingCompanyId);
            ViewData["Seller"] = await _context.Users.FirstOrDefaultAsync(s => s.Id == offer.SellerId);
            ViewData["Customer"] = await _context.Users.FirstOrDefaultAsync(s => s.Id == order.CustomerId);
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
            ViewData["ShippingCompany"] = new SelectList(_context.ShippingCompanies, "Id", "Name");
            return View();
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OfferId,Price,Quantity," +
            "CustomerId,TransactionId,StatusId,DateOfOrder,PaymentMethodId,Comment," +
            "ShippingCompanyId, ArrivalCountry, ArrivalCity, ArrivalStreet, ArrivalBuildingNumber, ArrivalZipCode")] OrderItemCreationForm orderItemForm)
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

                Shipping ship = new Shipping
                {
                    DateStarted = orderItemForm.DateOfOrder,
                    DepartmentCountry = "1",
                    DepartmentCity = "1",
                    DepartmentStreet = "1",
                    DepartmentBuildingNumber = "1",
                    DepartmentZipCode = "1",
                    ShippingCompanyId = orderItemForm.ShippingCompanyId,
                    ArrivalCountry = orderItemForm.ArrivalCountry,
                    ArrivalCity = orderItemForm.ArrivalCity,
                    ArrivalBuildingNumber = orderItemForm.ArrivalBuildingNumber,
                    ArrivalStreet = orderItemForm.ArrivalStreet,
                    ArrivalZipCode = orderItemForm.ArrivalZipCode,
                    OrderId = orderId,
                };
                _context.Add(ship);
                await _context.SaveChangesAsync();

                var of = _context.Offers.FirstOrDefault(o => o.Id == orderItemForm.OfferId);
                var cust = _context.Users.FirstOrDefault(u => u.Id == order.CustomerId);

                Notification notif = new Notification
                {
                    Title = "Створено нове замовлення!",
                    Text = "Користувач " + cust.Username + " замовив товар " + of.Name + "!",
                    TimeAdded = DateTime.Now,
                    ClassId = 6,
                    IsWatched = false,
                    UserId = of.SellerId,
                };

                _context.Notifications.Add(notif);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            var offer = _context.Offers.FirstOrDefault(o => o.Id == orderItemForm.OfferId);
            ViewData["UserId"] = orderItemForm.CustomerId;
            ViewData["Offer"] = offer;
            ViewData["PaymentMethodName"] = new SelectList(_context.PaymentMethods, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "Id", "Id");
            ViewData["OfferId"] = new SelectList(new List<Offer> { offer }, "Id", "Name");
            ViewData["ShippingCompany"] = new SelectList(_context.ShippingCompanies, "Id", "Name");
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

            var ship = await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderItem.OrderId);

            var order = await _context.Orders.FirstOrDefaultAsync(s => s.Id == orderItem.OrderId);

            OrderItemForm orderItemForm = new OrderItemForm()
            { 
                OfferId = orderItem.OfferId,
                OrderId = orderItem.OrderId,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity,
                CustomerId = order.CustomerId,
                TransactionId = order.TransactionId,
                StatusId = order.StatusId,
                DateOfOrder = order.DateOfOrder,
                PaymentMethodId = order.PaymentMethodId,
                Comment = order.Comment,
                ShippingCompanyId = ship.ShippingCompanyId,
                ArrivalCountry = ship.ArrivalCountry,
                ArrivalCity = ship.ArrivalCity,
                ArrivalStreet = ship.ArrivalStreet,
                ArrivalBuildingNumber = ship.ArrivalBuildingNumber,
                ArrivalZipCode = ship.ArrivalZipCode,
                DepartmentCountry = ship.DepartmentCountry,
                DepartmentCity = ship.DepartmentCity,
                DepartmentStreet = ship.DepartmentStreet,
                DepartmentBuildingNumber = ship.DepartmentBuildingNumber,
                DepartmentZipCode = ship.DepartmentZipCode,
            };

            ViewData["StatusName"] = new SelectList(_context.OrderStatuses, "Id", "Name");
            ViewData["PaymentMethodName"] = new SelectList(_context.PaymentMethods, "Id", "Name");
            ViewData["ShippingCompany"] = new SelectList(_context.ShippingCompanies, "Id", "Name");
            return View(orderItemForm);
        }

        // POST: OrderItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OfferId,Price,Quantity," +
            "CustomerId,TransactionId,StatusId,DateOfOrder,PaymentMethodId,Comment," +
            "ShippingCompanyId, ArrivalCountry, ArrivalCity, ArrivalStreet, ArrivalBuildingNumber, ArrivalZipCode," +
            "DepartmentCountry, DepartmentCity, DepartmentStreet, DepartmentBuildingNumber, DepartmentZipCode")] OrderItemForm orderItemForm)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == orderItemForm.OrderId);

                int prevStatusId = order.StatusId;

                order.Id = orderItemForm.OrderId;
                order.CustomerId = orderItemForm.CustomerId;
                order.TransactionId = orderItemForm.TransactionId;
                order.StatusId = orderItemForm.StatusId;
                order.DateOfOrder = orderItemForm.DateOfOrder;
                order.PaymentMethodId = orderItemForm.PaymentMethodId;
                order.Comment = orderItemForm.Comment;

                _context.Update(order);

                OrderItem oi = new OrderItem
                {
                    Id = id,
                    OrderId = orderItemForm.OrderId,
                    OfferId = orderItemForm.OfferId,
                    Price = orderItemForm.Price,
                    Quantity = orderItemForm.Quantity,
                };
                _context.Update(oi);

                var ship = await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderItemForm.OrderId);

                ship.DateStarted = orderItemForm.DateOfOrder;
                ship.DepartmentCountry = orderItemForm.DepartmentCountry;
                ship.DepartmentCity = orderItemForm.DepartmentCity;
                ship.DepartmentStreet = orderItemForm.DepartmentStreet;
                ship.DepartmentBuildingNumber = orderItemForm.DepartmentBuildingNumber;
                ship.DepartmentZipCode = orderItemForm.DepartmentZipCode;
                ship.ShippingCompanyId = orderItemForm.ShippingCompanyId;
                ship.ArrivalCountry = orderItemForm.ArrivalCountry;
                ship.ArrivalCity = orderItemForm.ArrivalCity;
                ship.ArrivalBuildingNumber = orderItemForm.ArrivalBuildingNumber;
                ship.ArrivalStreet = orderItemForm.ArrivalStreet;
                ship.ArrivalZipCode = orderItemForm.ArrivalZipCode;
                ship.OrderId = orderItemForm.OrderId;

                _context.Update(ship);

                var status = _context.OrderStatuses.FirstOrDefault(o => o.Id == order.StatusId);

                if (prevStatusId != orderItemForm.StatusId) 
                {
                    Notification notif = new Notification
                    {
                        Title = "Зміна статусу",
                        Text = "Статус вашого замовлення з номером " + order.Id + " змінився на "+ status.Name + "!",
                        TimeAdded = DateTime.Now,
                        ClassId = 2,
                        IsWatched = false,
                        UserId = order.CustomerId,
                    };
                    _context.Add(notif);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Clients));
            }
            ViewData["StatusName"] = new SelectList(_context.OrderStatuses, "Id", "Name");
            ViewData["PaymentMethodName"] = new SelectList(_context.PaymentMethods, "Id", "Name");
            ViewData["ShippingCompany"] = new SelectList(_context.ShippingCompanies, "Id", "Name");
            return View(orderItemForm);
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
            var order = await _context.Orders.FindAsync(orderItem.OrderId);
            var shipping = await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == order.Id);

            if (shipping is not null)
            {
                _context.OrderItems.Remove(orderItem);
                //_context.Orders.Remove(order);
                //_context.Shippings.Remove(shipping);
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
