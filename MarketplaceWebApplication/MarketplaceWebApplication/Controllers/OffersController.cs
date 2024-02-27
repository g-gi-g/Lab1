using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceWebApplication.Data;
using Microsoft.AspNetCore.Http;
using MarketplaceWebApplication.Models;

using System.IO;

namespace MarketplaceWebApplication.Controllers
{
    public class OffersController : Controller
    {
        private readonly DbmarketplaceContext _context;

        public OffersController(DbmarketplaceContext context)
        {
            _context = context;
        }

        // GET: Offers
        public async Task<IActionResult> Index()
        {
            var dbmarketplaceContext = _context.Offers.Include(o => o.Category).Include(o => o.Seller);
            return View(await dbmarketplaceContext.ToListAsync());
        }

        public async Task<IActionResult> MainPageView()
        {
            var dbmarketplaceContext = _context.Offers.Include(o => o.Category).Include(o => o.Seller);
            return View(await dbmarketplaceContext.ToListAsync());
        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Category)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.OfferCategories, "Id", "Id");
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SellerId,Name,Price,Description,Photo,NumberOfOrders,CategoryId,TimeAdded,IsDeleted,IsHidden,ItemAmount")] OfferModel offerModel)
        {
            offerModel.TimeAdded = DateTime.Now;
            Offer offer = new Offer();
            if (ModelState.IsValid)
            {
                offer.Id = offerModel.Id;
                offer.Name = offerModel.Name;
                offer.Price = offerModel.Price;
                offer.Description = offerModel.Description;
                offer.CategoryId = offerModel.CategoryId;
                offer.SellerId = offerModel.SellerId;
                offer.NumberOfOrders = offerModel.NumberOfOrders;
                offer.IsDeleted = offerModel.IsDeleted;
                offer.IsHidden = offerModel.IsHidden;
                offer.ItemAmount = offerModel.ItemAmount;
                offer.TimeAdded = offerModel.TimeAdded;

                Stream stream = offerModel.Photo.OpenReadStream();
                BinaryReader reader = new BinaryReader(stream);
                offer.Photo = reader.ReadBytes((int)stream.Length);
                reader.Close();
                stream.Close();

                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.OfferCategories, "Id", "Id", offer.CategoryId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", offer.SellerId);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.OfferCategories, "Id", "Id", offer.CategoryId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", offer.SellerId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SellerId,Name,Price,Description,Photo,NumberOfOrders,CategoryId,TimeAdded,IsDeleted,IsHidden,ItemAmount")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.OfferCategories, "Id", "Id", offer.CategoryId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", offer.SellerId);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Category)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.Id == id);
        }
    }
}
