using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Models;
using MarketplaceWebApplication.Extensions;

namespace MarketplaceWebApplication.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly DbmarketplaceContext _context;

        public FeedbacksController(DbmarketplaceContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index(int id)
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is not null)
            {
                ViewData["UserId"] = userInfo.Id;
            }
            else
            {
                ViewData["UserId"] = 0;
            }

            var dbmarketplaceContext = _context.Feedbacks.Include(f => f.Offer)
                .Include(f => f.User)
                .Where(f => f.Offer.Id == id);

            ViewData["OfferId"] = id;
            return View(await dbmarketplaceContext.ToListAsync());
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Offer)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create(int Id)
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            string userId = userInfo.Id;

            var offer = _context.Offers.FirstOrDefault(o => o.Id == Id);
            ViewData["OfferId"] = new SelectList(new List<Offer> { offer }, "Id", "Id");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewData["UserId"] = new SelectList(new List<User> { user }, "Id", "Id");

            ViewData["User"] = user.Id;
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rating,Text,OfferId,UserId,TimeAdded")] FeedbackModel feedback)
        {
            feedback.TimeAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                Feedback fb = new Feedback 
                {
                    OfferId = feedback.OfferId,
                    Rating = feedback.Rating,
                    UserId = feedback.UserId,
                    Text = feedback.Text,
                    TimeAdded = feedback.TimeAdded,
                };
                _context.Add(fb);

                var of = await _context.Offers.FirstOrDefaultAsync(o => o.Id == fb.OfferId);

                Notification notif = new Notification
                {
                    Title = "Ви залишили відгук",
                    Text = "Ви залишили відгук на пропозицію " + of.Name + "!",
                    TimeAdded = DateTime.Now,
                    ClassId = 4,
                    IsWatched = false,
                    UserId = fb.UserId,
                };
                _context.Add(notif);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = fb.OfferId });
            }

            var offer = _context.Offers.FirstOrDefault(o => o.Id == feedback.OfferId);
            ViewData["OfferId"] = new SelectList(new List<Offer> { offer }, "Id", "Id");

            var user = _context.Users.FirstOrDefault(u => u.Id == feedback.UserId);
            ViewData["UserId"] = new SelectList(new List<User> { user }, "Id", "Id");
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", feedback.OfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", feedback.UserId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rating,Text,OfferId,UserId,TimeAdded")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", feedback.OfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", feedback.UserId);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.Offer)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = feedback.OfferId });
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}
