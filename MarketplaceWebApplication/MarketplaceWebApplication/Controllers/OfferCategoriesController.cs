using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketplaceWebApplication.Data;
using Microsoft.AspNetCore.Authorization;

namespace MarketplaceWebApplication.Controllers;

[Authorize(Roles = "admin")]
public class OfferCategoriesController : Controller
{
    private readonly DbmarketplaceContext _context;

    public OfferCategoriesController(DbmarketplaceContext context)
    {
        _context = context;
    }

    // GET: OfferCategories
    public async Task<IActionResult> Index()
    {
        return View(await _context.OfferCategories.ToListAsync());
    }

    // GET: OfferCategories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: OfferCategories/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] OfferCategory offerCategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(offerCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(offerCategory);
    }

    // GET: OfferCategories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var offerCategory = await _context.OfferCategories.FindAsync(id);
        if (offerCategory == null)
        {
            return NotFound();
        }
        return View(offerCategory);
    }

    // POST: OfferCategories/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] OfferCategory offerCategory)
    {
        if (id != offerCategory.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(offerCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfferCategoryExists(offerCategory.Id))
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
        return View(offerCategory);
    }

    // GET: OfferCategories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var offerCategory = await _context.OfferCategories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (offerCategory == null)
        {
            return NotFound();
        }

        return View(offerCategory);
    }

    // POST: OfferCategories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var offerCategory = await _context.OfferCategories.FindAsync(id);
        if (offerCategory != null)
        {
            _context.OfferCategories.Remove(offerCategory);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OfferCategoryExists(int id)
    {
        return _context.OfferCategories.Any(e => e.Id == id);
    }
}
