using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public static List<Country> GetCountries()
        {
            return new List<Country> { 
                new Country
                {
                    Id = 1,
                    Name = "England"
                },
                new Country
                {
                    Id = 1,
                    Name = "Vietnam"
                }
            };
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Country.GetCountries());
        }
    }
}
