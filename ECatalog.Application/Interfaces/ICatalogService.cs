using ECatalog.Application.Common;
using ECatalog.Application.DTO;
using ECatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<Result<PagedResult<CatalogItemDTO>>> GetAllAsync(int? page = null, int? pageSize = null, string? filter = null);
        //Task<PagedResult<CatalogItem>> GetAllAsync(int? page = null, int? pageSize = null, string? filter = null);
        Task<Result<CatalogItemDTO?>> GetByIdAsync(Guid id);
        Task<Result<CatalogItemDTO>> CreateAsync(CreateCatalogItemDTO dto);
        Task<Result<bool>> UpdateAsync(UpdateCatalogItemDTO dto);
        Task<Result<bool>> DeleteAsync(Guid id);
    }
}
