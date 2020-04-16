using System.Threading.Tasks;
using Covid19.Client;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamplesController: ControllerBase
    {

        private readonly ICovid19Client _covid19Client;

        public ExamplesController(ICovid19Client covid19Client)
        {
            _covid19Client = covid19Client;
        }

        [HttpGet]
        [Route("locations")]
        public async Task<ActionResult> GetLocations()
        {
            var locations = await _covid19Client.GetLocationsAsync();
            return Ok(locations);
        }
    }
}