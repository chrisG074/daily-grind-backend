using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    [HttpPost("products/csv")]
    [Authorize(Roles = "Vertaler,Systeembeheerder")]
    public IActionResult ExportCsv()
    {
        var csvContent = "sku,name_en,name_nl,name_de,name_fr,description\n" +
                         "FLAG_NL,Netherlands Flag,Nederlandse Vlag,Niederlande Flagge,Drapeau des Pays-Bas,100x150cm spun-poly";

        var bytes = Encoding.UTF8.GetBytes(csvContent);
        return File(bytes, "text/csv", "products_export.csv");
    }
}
