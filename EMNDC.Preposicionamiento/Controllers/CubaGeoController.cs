using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CubaGeoController : ControllerBase
{
    private readonly CubaGeoService _geoService;

    public CubaGeoController(CubaGeoService geoService)
    {
        _geoService = geoService;
    }

    [HttpGet("provincias")]
    public IActionResult GetProvincias() => Ok(_geoService.GetAllProvincias());
}
