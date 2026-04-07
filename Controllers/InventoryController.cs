using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    [HttpGet("alerts")]
    [Authorize(Roles = "Magazijnmedewerker,Systeembeheerder")]
    public IActionResult GetAlerts()
    {
        var alerts = new
        {
            Message = "Voorraad waarschuwingen getrokken.",
            ItemsToRestock = new object[]
            {
                new { MaterialCode = "POLY_110g", Name = "Polyesterdoek Standaard", Stock = 4.5, Threshold = 10, Unit = "m" },
                new { MaterialCode = "INK_MAG", Name = "Magenta Inkt", Stock = 1.0, Threshold = 2, Unit = "liters" }
            }
        };
        return Ok(alerts);
    }
}
