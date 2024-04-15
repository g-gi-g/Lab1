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
    public class SavedOffersController : Controller
    {
        private readonly DbmarketplaceContext _context;

        public SavedOffersController(DbmarketplaceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Save(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SavedOffer saved = new SavedOffer();
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");
            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            saved.OfferId = (int)id;
            saved.UserId = userInfo.Id;
            saved.TimeAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(saved);
                await _context.SaveChangesAsync();

                var of = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);

                Notification notif = new Notification
                {
                    Title = "Ви зберігли пропозицію",
                    Text = "Ви зберігли пропозицію " + of.Name + "!",
                    TimeAdded = DateTime.Now,
                    ClassId = 3,
                    IsWatched = false,
                    UserId = saved.UserId,
                };

                _context.Add(notif);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // GET: SavedOffers
        public async Task<IActionResult> Index()
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            string userId = userInfo.Id;

            var dbmarketplaceContext = _context.SavedOffers
                .Include(s => s.Offer).ThenInclude(s => s.Seller)
                .Include(s => s.User)
                .Where(s => s.UserId == userId);

            //List<OffersRatings> ORList = new List<OffersRatings>();

            //foreach (var o in dbmarketplaceContext)
            //{
                //var rating = await RatingCounter(o.OfferId);
                //ORList.Add(rating);
            //}

            //ViewData["ORList"] = ORList;

            return View(await dbmarketplaceContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchWord)
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            string userId = userInfo.Id;

            ViewData["SearchWord"] = searchWord;

            //List<OffersRatings> ORList = new List<OffersRatings>();

            if (String.IsNullOrEmpty(searchWord)) 
            {
                var dbmarketplaceContext = _context.SavedOffers
                    .Include(s => s.Offer).ThenInclude(s => s.Seller)
                    .Include(s => s.Offer).ThenInclude(o => o.Category)
                    .Include(s => s.User)
                    .Where(s => s.UserId == userId);

                //foreach (var o in dbmarketplaceContext)
                //{
                    //var rating = await RatingCounter(o.OfferId);
                    //ORList.Add(rating);
                //}

                //ViewData["ORList"] = ORList;

                return View(await dbmarketplaceContext.ToListAsync());
            }
            else 
            {
                var dbmarketplaceContext = _context.SavedOffers
                    .Include(s => s.Offer).ThenInclude(s => s.Seller)
                    .Include(s => s.Offer).ThenInclude(o => o.Category)
                    .Include(s => s.User)
                    .Where(s => s.Offer.Name == searchWord)
                    .Where(s => s.UserId == userId);

                //foreach (var o in dbmarketplaceContext)
                //{
                    //var rating = await RatingCounter(o.OfferId);
                    //ORList.Add(rating);
                //}

                //ViewData["ORList"] = ORList;

                return View(await dbmarketplaceContext.ToListAsync());
            }
        }

        // GET: SavedOffers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savedOffer = await _context.SavedOffers
                .Include(s => s.Offer)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (savedOffer == null)
            {
                return NotFound();
            }

            return View(savedOffer);
        }

        // GET: SavedOffers/Create
        public IActionResult Create()
        {
            var userInfo = HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails");

            if (userInfo is null)
            {
                return RedirectToAction("NotLoggedView", "Home", null);
            }

            string userId = userInfo.Id;

            ViewData["UserId"] = userId;

            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SavedOffers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,OfferId,TimeAdded")] SavedOffer savedOffer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(savedOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", savedOffer.OfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", savedOffer.UserId);
            return View(savedOffer);
        }

        // GET: SavedOffers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savedOffer = await _context.SavedOffers.FindAsync(id);
            if (savedOffer == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", savedOffer.OfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", savedOffer.UserId);
            return View(savedOffer);
        }

        // POST: SavedOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OfferId,TimeAdded")] SavedOffer savedOffer)
        {
            if (id != savedOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(savedOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavedOfferExists(savedOffer.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", savedOffer.OfferId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", savedOffer.UserId);
            return View(savedOffer);
        }

        // GET: SavedOffers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savedOffer = await _context.SavedOffers
                .Include(s => s.Offer)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (savedOffer == null)
            {
                return NotFound();
            }

            return View(savedOffer);
        }

        // POST: SavedOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var savedOffer = await _context.SavedOffers.FindAsync(id);
            if (savedOffer != null)
            {
                _context.SavedOffers.Remove(savedOffer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SavedOfferExists(int id)
        {
            return _context.SavedOffers.Any(e => e.Id == id);
        }

        private async Task<OffersRatings> RatingCounter(int offerId)
        {
            int feedbacksCount = _context.Feedbacks.Count(o => o.OfferId == offerId);
            if (feedbacksCount == 0) return new OffersRatings { OfferId = offerId, Rating = -1 };
            var ratingsSum = _context.Feedbacks.Where(f => f.OfferId == offerId)
                .Sum(f => f.Rating);
            double meanRating = (double)ratingsSum / feedbacksCount;
            return new OffersRatings { OfferId = offerId, Rating = meanRating };
        }
    }
}
