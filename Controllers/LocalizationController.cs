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
        var currencies = new[]
        {
            new { code = "EUR", symbol = "€", exchangeRate = 1.0 },
            new { code = "GBP", symbol = "£", exchangeRate = 0.85 },
            new { code = "USD", symbol = "$", exchangeRate = 1.1 }
        };

        return Ok(currencies);
    }

    [HttpGet("translations")]
    public IActionResult GetTranslations()
    {
        var language = HttpContext.Items["Language"]?.ToString() ?? "en";
        language = language.ToLowerInvariant();

        var filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Data", "translations.json");
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Translations file not found.");
        }

        var jsonString = System.IO.File.ReadAllText(filePath);
        var translationsData = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Text.Json.JsonElement>>>(jsonString);

        var result = new System.Collections.Generic.Dictionary<string, object>();

        if (translationsData != null)
        {
            foreach (var category in translationsData)
            {
                if (category.Value.TryGetValue(language, out var langData))
                {
                    result[category.Key] = langData;
                }
                else if (category.Value.TryGetValue("en", out var fallbackData))
                {
                    result[category.Key] = fallbackData;
                }
            }
        }

        return Ok(result);
    }
}