using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Extensions;
using MarketplaceWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.IO;

namespace MarketplaceWebApplication.WebMVC.Infrastructure.Services;

public class OfferImportService : IImportService<OfferModel>
{
    private readonly DbmarketplaceContext _context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private string userId;

    public OfferImportService(DbmarketplaceContext context, string userId, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        this.userId = userId;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream is not readable", nameof(stream));
        }
        using var workBook = new XLWorkbook(stream);
        var worksheet = workBook.Worksheets.FirstOrDefault();
        if (worksheet is null)
        {
            return;
        }
        foreach (var rows in worksheet.RowsUsed().Skip(1)) 
        {
            await AddOfferAsync(rows, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task AddOfferAsync(IXLRow row, CancellationToken cancellationToken)
    {
        var offerName = GetOfferName(row);

        var offer = await _context.Offers.FirstOrDefaultAsync(offer => offer.Name == offerName, cancellationToken);

        if (offer == null) 
        {
            offer = new Offer();
            offer.Name = offerName;
            offer.SellerId = userId;
            offer.NumberOfOrders = 0;
            offer.Price = GetOfferPrice(row);
            offer.Description = GetOfferDescription(row);
            offer.Photo = GetOfferPhoto();
            offer.TimeAdded = GetOfferTime();
            offer.IsDeleted = GetOfferDeleted(row);
            offer.IsHidden = GetOfferHidden(row);
            offer.ItemAmount = GetOfferItemAmount(row);
            _context.Offers.Add(offer);
        }

        if (offer.Category is null)
        {
            offer.Category = await GetOfferCategoryAsync(row, cancellationToken);
        }
    }

    private static string GetOfferName(IXLRow row)
    {
        return row.Cell(1).GetValue<string>();
    }

    private static int GetOfferPrice(IXLRow row)
    {
        int price = row.Cell(2).GetValue<int>();
        if (price >= 0) return price;
        return 0;
    }

    private static string GetOfferDescription(IXLRow row)
    {
        return row.Cell(3).GetValue<string>();
    }

    private byte[] GetOfferPhoto()
    {
        string webRootPath = _webHostEnvironment.WebRootPath;
        string imagePath = Path.Combine(webRootPath, "images", "EmptyPhoto.png");
        return File.ReadAllBytes(imagePath);
    }

    private static DateTime GetOfferTime()
    {
        return DateTime.Now;
    }

    private static bool GetOfferDeleted(IXLRow row)
    {
        return row.Cell(4).GetValue<bool>(); ;
    }

    private static bool GetOfferHidden(IXLRow row)
    {
        return row.Cell(5).GetValue<bool>(); ;
    }

    private static int GetOfferItemAmount(IXLRow row)
    {
        int amount = row.Cell(6).GetValue<int>();
        if (amount > 0) return amount;
        return 1;
    }

    private async Task<OfferCategory> GetOfferCategoryAsync(IXLRow row, CancellationToken cancellationToken)
    {
        string catName = row.Cell(7).GetValue<string>();
        OfferCategory? cat = await
        _context.OfferCategories.FirstOrDefaultAsync(cat =>
        cat.Name == catName, cancellationToken);
        if (cat is null)
        {
            cat = new OfferCategory();
            cat.Name = catName;
            cat.Description = "Description";
            _context.Add(cat);
            return cat;
        }
        return cat;
    }
}

