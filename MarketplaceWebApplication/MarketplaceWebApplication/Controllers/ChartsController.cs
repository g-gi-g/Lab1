using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountFeedbacksByRatingResponseItem(string Rating, int Count);

        private readonly DbmarketplaceContext _context;

        public ChartsController(DbmarketplaceContext _context)
        { 
            this._context = _context;
        }

        [HttpGet("countByRating")]
        public async Task<JsonResult> GetCountByRatingAsync(CancellationToken cancellationToken)
        { 
            var responseItems = await _context
                .Feedbacks
                //.Where(fb => fb.OfferId == offerId)
                .GroupBy(fb => fb.Rating)
                .Select(group => new 
                CountFeedbacksByRatingResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
    }
}
