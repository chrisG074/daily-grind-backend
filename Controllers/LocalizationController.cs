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

    [HttpGet("translations")]
    public IActionResult GetTranslations([FromQuery] string? lang = null)
    {
        // Use provided lang query parameter, fallback to middleware injected language, default to "en"
        var language = lang ?? HttpContext.Items["Language"]?.ToString() ?? "en";
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