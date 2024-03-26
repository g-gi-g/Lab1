using MarketplaceWebApplication.Models;

namespace MarketplaceWebApplication.WebMVC.Infrastructure.Services;

public interface IExportService<TEntity>
	where TEntity : Entity
{
	Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
}
