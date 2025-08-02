using ECatalog.Application;
using ECatalog.Application.Common;
using ECatalog.Application.DTO;
using ECatalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                Result<IEnumerable<CatalogItemDTO>> result = await _catalogService.GetAllAsync();
                return Ok(result.Data);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Result<CatalogItemDTO?> result = await _catalogService.GetByIdAsync(id);
                return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogItemDTO parameter)
        {
            try
            {
                Result<CatalogItemDTO?> result = await _catalogService.CreateAsync(parameter);
                return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result.Data) : BadRequest(result.ErrorMessage);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error.");
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

                    return result.IsSuccess ? Ok("Update success.") : BadRequest(result.ErrorMessage);
                }
                else
                    return BadRequest("Cannot process update, invalid request.");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result<bool> result = await _catalogService.DeleteAsync(id);
            if (result.IsNotFound)
                return NotFound("Catalog doesnt exist.");

            return result.IsSuccess ? Ok("Delete success.") : StatusCode(500, result.ErrorMessage);
        }

    }
}
