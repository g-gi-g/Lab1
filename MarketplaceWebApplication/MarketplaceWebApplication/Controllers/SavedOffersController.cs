﻿using System;
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
            saved.UserId = (int)userInfo.Id;
            saved.TimeAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(saved);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // GET: SavedOffers
        public async Task<IActionResult> Index()
        {
            var dbmarketplaceContext = _context.SavedOffers.Include(s => s.Offer).Include(s => s.User);
            return View(await dbmarketplaceContext.ToListAsync());
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

            int userId = (int)HttpContext.Session.GetObjectFromJson<UserDetails>("UserDetails").Id;
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
    }
}
