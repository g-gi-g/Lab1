using MarketplaceWebApplication.Data;
using MarketplaceWebApplication.Models;

namespace MarketplaceWebApplication.WebMVC.Infrastructure.Services;

public class OfferDataPortServiceFactory : IDataPortServiceFactory<OfferModel>
{
    private readonly DbmarketplaceContext _context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private string userId;

    public OfferDataPortServiceFactory(DbmarketplaceContext _context, string userId, IWebHostEnvironment webHostEnvironment)
    {
        this._context = _context;
        this.userId = userId;
        _webHostEnvironment = webHostEnvironment;
    }
    public IExportService<OfferModel> GetExportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet")
        {
            return new OfferExportService(_context, userId);
        }
        throw new NotImplementedException($"No export service implemented for offers with content type { contentType }");
    }
    public IImportService<OfferModel> GetImportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new OfferImportService(_context, userId, _webHostEnvironment);
        }
        throw new NotImplementedException($"No import service implemented for offers with content type { contentType}");
    }
}
