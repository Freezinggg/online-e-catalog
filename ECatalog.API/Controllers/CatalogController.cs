using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            //TO-DO: Fetch from service/repo
            return Ok(new[] { "Item 1", "Item 2" });
        }

        [HttpPost]
        public IActionResult Create([FromBody] object dto)
        {
            //TO-DO: Add/create to service/repo
            return CreatedAtAction(nameof(GetAll), null);
        }

    }
}
    