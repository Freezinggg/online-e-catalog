using ECatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Interfaces
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<CatalogItem>> GetAllAsync();
        Task<CatalogItem?> GetByIdAsync(Guid id);
        Task<CatalogItem?> CreateAsync(CatalogItem item);
        Task<bool> UpdateAsync(CatalogItem item);
        Task<bool> DeleteAsync(Guid id);
    }
}
