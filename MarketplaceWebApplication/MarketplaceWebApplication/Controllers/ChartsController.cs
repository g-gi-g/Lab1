using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceWebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartsController : ControllerBase
{
    private record CountOffersByCategoriesResponseItem(string Cat, int Count);

    private readonly DbmarketplaceContext _context;

    public ChartsController(DbmarketplaceContext _context)
    { 
        this._context = _context;
    }

    [HttpGet("countOffersByCats")]
    public async Task<JsonResult> GetOffersCountByCategoriesAsync(CancellationToken cancellationToken)
    { 
        var responseItems = await _context
            .Offers
            .Where(o => !o.IsHidden && !o.IsDeleted)
            .GroupBy(o => o.CategoryId)
            .Select(group => new
            CountOffersByCategoriesResponseItem(group.Key.ToString(), group.Count()))
            .ToListAsync(cancellationToken);

        return new JsonResult(responseItems);
    }

    [HttpGet("countOffersByUsers")]
    public async Task<JsonResult> GetOffersCountByUsersAsync(CancellationToken cancellationToken)
    {
        var responseItems = await _context
            .Offers
            //.Where(o => !o.IsHidden && !o.IsDeleted)
            .GroupBy(o => o.SellerId)
            .Select(group => new
            CountOffersByCategoriesResponseItem(group.Key.ToString(), group.Count()))
            .ToListAsync(cancellationToken);

        return new JsonResult(responseItems);
    }
}
