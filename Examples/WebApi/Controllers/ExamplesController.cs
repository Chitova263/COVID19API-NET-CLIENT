using System.Threading.Tasks;
using Covid19API.Web;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamplesController: ControllerBase
    {
        private readonly ICovid19WebAPI aPI;

        public ExamplesController(ICovid19WebAPI aPI)
        {
            this.aPI = aPI ?? throw new System.ArgumentNullException(nameof(aPI));
        }

        [HttpGet]
        [Route("locations")]
        public async Task<ActionResult> GetLocations()
        {
            var locations = await this.aPI.GetLocationsAsync();
            return Ok(locations);
        }
    }
}