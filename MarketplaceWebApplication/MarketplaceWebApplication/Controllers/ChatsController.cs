using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceWebApplication.Data;
using Microsoft.AspNetCore.Http;
using MarketplaceWebApplication.Extensions;
using MarketplaceWebApplication.Models;

namespace MarketplaceWebApplication.Controllers
{
    public class ChatsController : Controller
    {
        private readonly DbmarketplaceContext _context;

        public ChatsController(DbmarketplaceContext context)
        {
            _context = context;
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            int? userId = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;

            var AsASeller = _context.Chats.Include(c => c.Offer)
                .Where(c => c.Offer.SellerId == userId)
                .ToList();

            var SellerChatIds = AsASeller.Select(chat => chat.Id).ToHashSet();

            var BuyersChats = _context.Messages.Where(m => SellerChatIds.Contains(m.ChatId) && m.SenderId != userId)
                .Select(m => new ChatBuyer
                {
                    ChatId = m.ChatId,
                    BuyerLogin = m.Sender.Username,
                })
                .ToList();

            var AsABuyer = _context.Chats.Include(c => c.Offer)
                .Where(c => c.Offer.SellerId != userId)
                .ToList();

            UserChats userChats = new UserChats
            {
                AsASeller = AsASeller,
                AsABuyer = AsABuyer,
                ChatsBuyers = BuyersChats
            };

            return View(userChats);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIndex(bool isSelected, UserChats userChats)
        {
            if (isSelected)
            {
                ViewData["Message"] = "BuyerChatsSelected";
            }
            else
            {
                ViewData["Message"] = "SellerChatsSelected";
            }
            return View("~/Views/Shared/SomeOtherView.cshtml", userChats);
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(c => c.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: Chats/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id is null) 
            {
                return NotFound();
            }
            Chat chat = new Chat();
            chat.OfferId = (int)id;
            chat.TimeCreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
            //ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id");
            //return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OfferId,TimeCreated")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", chat.OfferId);
            return View(chat);
        }

        // GET: Chats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", chat.OfferId);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OfferId,TimeCreated")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", chat.OfferId);
            return View(chat);
        }

        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(c => c.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
