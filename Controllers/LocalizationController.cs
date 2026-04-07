using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    [HttpGet("languages")]
    public IActionResult GetLanguages()
    {
        var languages = new[]
        {
            new { Code = "nl", Name = "Nederlands" },
            new { Code = "de", Name = "Duits" },
            new { Code = "en", Name = "Engels" },
            new { Code = "fr", Name = "Frans" }
        };

        return Ok(languages);
    }

    [HttpGet("currencies")]
    public IActionResult GetCurrencies()
    {
        var currencies = new[] { "EUR", "GBP", "USD" };

        return Ok(currencies);
    }
}