using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class MarketingController : ControllerBase
{
    [HttpGet("campaigns/active")]
    public IActionResult GetActiveCampaigns()
    {
        var region = HttpContext.Items["Region"]?.ToString() ?? "NL";
        var promotions = new List<object>();

        if (region == "NL")
        {
            promotions.Add(new { Id = 1, Type = "banner", Name = "Koningsdag Promo", ImgUrl = "/uploads/koningsdag.png" });
        }
        else if (region == "DE")
        {
            promotions.Add(new { Id = 2, Type = "banner", Name = "Tag der Deutschen Einheit", ImgUrl = "/uploads/de_einheit.png" });
        }

        return Ok(new { Region = region, Banners = promotions });
    }
}
