using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceWebApplication.WebMVC.Infrastructure.Services;

public class OfferExportService : IExportService<OfferModel>
{
	private const string RootWorksheetName = "Offers";

	private string userId;

	private static readonly IReadOnlyList<string> HeaderNames = new string[]
	{
		 "Назва",
		 "Ціна",
		 "Дата",
		 "Опис",
		 "Чи видалено",
		 "Чи приховано",
		 "Кількість товару",
		 "Категорія"
	};

	private readonly DbmarketplaceContext _context;

	private static void WriteHeader(IXLWorksheet worksheet)
	{
		for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
		{
			worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
		}
		worksheet.Row(1).Style.Font.Bold = true;
	}

	private static string GetNameOrDefault(OfferCategory? category)
	{
		if (category is null)
		{
			return string.Empty;
		}
		return $"{category.Name}";
	}


	private static void WriteMovie(IXLWorksheet worksheet, Offer offer, int rowIndex)
	{
		var columnIndex = 1;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.Name;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.Price;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.TimeAdded;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.Description;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.IsDeleted;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.IsHidden;
		worksheet.Cell(rowIndex, columnIndex++).Value = offer.ItemAmount;
		worksheet.Cell(rowIndex, columnIndex++).Value = GetNameOrDefault(offer.Category);
	}

	private static void WriteMovies(IXLWorksheet worksheet, ICollection<Offer> offers)
	{
		WriteHeader(worksheet);
		int rowIndex = 2;
		foreach (var offer in offers)
		{
			WriteMovie(worksheet, offer, rowIndex);
			rowIndex += 1;
		}
	}

	public OfferExportService(DbmarketplaceContext context, string userId)
	{
		_context = context;
		this.userId = userId;
	}

	public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
	{
		if (!stream.CanWrite)
		{
			throw new ArgumentException("Input stream is not writable");
	    }
		var offers = await _context.Offers
			.Include(offer => offer.Category)
			.Where(offer => offer.SellerId == userId)
			.ToListAsync(cancellationToken);
		using var workbook = new XLWorkbook();
		var worksheet = workbook.Worksheets.Add(RootWorksheetName);
		WriteMovies(worksheet, offers);
		workbook.SaveAs(stream);
	}
}
