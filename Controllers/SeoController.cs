using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SeoController : ControllerBase
{
    [HttpGet("{productId}/metadata")]
    public IActionResult GetMetadata(string productId)
    {
        var language = HttpContext.Items["Language"]?.ToString() ?? "nl";
        var langUpper = language.ToUpper();

        return Ok(new
        {
            ProductId = productId,
            Language = language,
            SeoDescription = $"Koop de beste vlaggen uit {langUpper}, top kwaliteit en scherpe prijzen!",
            SeoTitle = $"Vlag {productId} ({langUpper}) | Vlag en Wimpel Webshop",
            Keywords = new[] { "vlaggen", "kopen", "goedkoop" }
        });
    }
}
