using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entities;
using ECatalog.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Infrastructure.Persistence
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogDbContext _db;
        public CatalogRepository(CatalogDbContext db)
        {
            _db = db;
        }

        public async Task<CatalogItem?> CreateAsync(CatalogItem item)
        {
            try
            {
                _db.Add(item);
                await _db.SaveChangesAsync();

                return item;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                CatalogItem? item = await _db.CatalogItems.FindAsync(id);
                if (item != null)
                {
                    _db.CatalogItems.Remove(item);
                    await _db.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<IEnumerable<CatalogItem>> GetAllAsync()
        {
            return await _db.CatalogItems.AsNoTracking().ToListAsync();
        }

        public async Task<CatalogItem?> GetByIdAsync(Guid id)
        {
            return await _db.CatalogItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(CatalogItem item)
        {
            try
            {
                if(item != null)
                {
                    _db.CatalogItems.Update(item);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
