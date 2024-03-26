using MarketplaceWebApplication.Models;

namespace MarketplaceWebApplication.WebMVC.Infrastructure.Services;

public interface IImportService<TEntity>
    where TEntity : Entity
{
    Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
} 

