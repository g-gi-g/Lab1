using MarketplaceWebApplication.Models;
using DocumentFormat.OpenXml.Vml.Office;

namespace MarketplaceWebApplication.WebMVC.Infrastructure.Services; 

public interface IDataPortServiceFactory<TEntity>
    where TEntity : Entity
{
    IImportService<TEntity> GetImportService(string contentType);

    IExportService<TEntity> GetExportService(string contentType);
}

