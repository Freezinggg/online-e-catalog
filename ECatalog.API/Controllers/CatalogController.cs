using ECatalog.Application;
using ECatalog.Application.Common;
using ECatalog.Application.DTO;
using ECatalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService, ILogger<CatalogController> logger)
        {
            _catalogService = catalogService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                Result<IEnumerable<CatalogItemDTO>> result = await _catalogService.GetAllAsync();
                //return Ok(result.Data);
                return Ok(ApiResponse<IEnumerable<CatalogItemDTO>>.Ok(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get All Catalog");
                return StatusCode(500, ApiResponse<object>.Fail("Get all catalog error, internal server problem."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Result<CatalogItemDTO?> result = await _catalogService.GetByIdAsync(id);
                return result.IsSuccess ? Ok(ApiResponse<CatalogItemDTO?>.Ok(result.Data)) : NotFound(ApiResponse<CatalogItemDTO?>.Fail(result.ErrorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get Catalog by Id");
                return StatusCode(500, ApiResponse<object>.Fail("Get catalog error, internal server problem."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogItemDTO parameter)
        {
            try
            {
                Result<CatalogItemDTO?> result = await _catalogService.CreateAsync(parameter);
                return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Data.Id }, ApiResponse<Guid>.Ok(result.Data.Id)) : BadRequest(ApiResponse<Guid>.Fail(result.ErrorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Create Catalog");
                return StatusCode(500, ApiResponse<object>.Fail("Creating catalog error, internal server problem."));
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCatalogItemDTO parameter)
        {
            try
            {
                if (id == parameter.Id)
                {
                    Result<bool> result = await _catalogService.UpdateAsync(parameter);
                    if (result.IsNotFound)
                        return NotFound("Catalog doesnt exist.");

                    return result.IsSuccess ? Ok(ApiResponse<object>.Ok("Update catalog success.")) : BadRequest(ApiResponse<object>.Fail(result.ErrorMessage));
                }
                else
                    return BadRequest(ApiResponse<object>.Fail("Cannot process update, invalid request."));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error Update Catalog");
                return StatusCode(500, ApiResponse<object>.Fail("Creating catalog error, internal server problem."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result<bool> result = await _catalogService.DeleteAsync(id);
            if (result.IsNotFound)
                return NotFound(ApiResponse<object>.Fail("Error deleting catalog."));

            return result.IsSuccess ? Ok(ApiResponse<object>.Ok("Delete catalog success.")) : StatusCode(500, ApiResponse<object>.Fail(result.ErrorMessage));
        }

    }
}
