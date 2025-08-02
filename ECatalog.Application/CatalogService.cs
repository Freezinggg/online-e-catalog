using ECatalog.Application.Common;
using ECatalog.Application.DTO;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _repo;
        public CatalogService(ICatalogRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<CatalogItemDTO?>> CreateAsync(CreateCatalogItemDTO dto)
        {
            try
            {
                CatalogItem item = new()
                {
                    Id = new Guid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageUrl = dto.ImageUrl,
                    InStock = dto.InStock,
                    CreateTime = DateTime.UtcNow
                };

                CatalogItem? result = await _repo.CreateAsync(item);
                if (result != null)
                {
                    return Result<CatalogItemDTO?>.Success(new CatalogItemDTO()
                    {
                        Id = item.Id,
                        Name = result.Name,
                        Description = result.Description,
                        ImageUrl = result.ImageUrl,
                        InStock = result.InStock
                    });
                }

                return Result<CatalogItemDTO?>.Fail("Creating catalog failed.");
            }
            catch
            {
                return Result<CatalogItemDTO?>.Error("Creating catalog failed due to internal server error.");
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                bool result = await _repo.DeleteAsync(id);
                if (result)
                    return Result<bool>.Success(true);
                else
                    return Result<bool>.Fail("Delete catalog failed.");
            }
            catch
            {
                return Result<bool>.Error("Delete catalog failed due to internal server error.");
            }
        }

        public async Task<Result<IEnumerable<CatalogItemDTO>>> GetAllAsync()
        {
            List<CatalogItemDTO> catalogItemDTOs = new();
            IEnumerable<CatalogItem> result = await _repo.GetAllAsync();
            foreach (CatalogItem item in result)
            {
                catalogItemDTOs.Add(new CatalogItemDTO()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    ImageUrl = item.ImageUrl,
                    InStock = item.InStock
                });
            }

            return Result<IEnumerable<CatalogItemDTO>>.Success(catalogItemDTOs);
        }

        public async Task<Result<CatalogItemDTO?>> GetByIdAsync(Guid id)
        {
            try
            {
                CatalogItem? item = await _repo.GetByIdAsync(id);
                if (item != null)
                {
                    CatalogItemDTO dto = new CatalogItemDTO()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        ImageUrl = item.ImageUrl,
                        InStock = item.InStock
                    };

                    return Result<CatalogItemDTO?>.Success(dto);
                }

                return Result<CatalogItemDTO?>.NotFound("Catalog doesnt exist.");
            }
            catch
            {
                return Result<CatalogItemDTO?>.Error("Failed retrieving catalog due to internal server error.");
            }

        }

        public async Task<Result<bool>> UpdateAsync(UpdateCatalogItemDTO dto)
        {
            CatalogItem? exist = await _repo.GetByIdAsync(dto.Id);
            if (exist != null)
            {
                CatalogItem updateCatalogItem = new()
                {
                    Id = dto.Id,
                    Name = string.IsNullOrEmpty(dto.Name) ? exist.Name : dto.Name,
                    Description = string.IsNullOrEmpty(dto.Description) ? exist.Description : dto.Description,
                    ImageUrl = string.IsNullOrEmpty(dto.ImageUrl) ? exist.ImageUrl : dto.ImageUrl,
                    InStock = dto.InStock,
                    UpdateTime = DateTime.UtcNow
                };

                bool result = await _repo.UpdateAsync(updateCatalogItem);
                if (result)
                    return Result<bool>.Success(true);
                else
                    return Result<bool>.Fail("Failed updating catalog.");
            }
            else
            {
                return Result<bool>.NotFound("Catalog doesnt exist.");
            }
        }
    }
}
