using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class IpDetectionMiddleware
{
    private readonly RequestDelegate _next;

    public IpDetectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress?.ToString() ?? "";
        }

        // Mock GeoIP lookup logic
        if (ip == "::1" || ip == "127.0.0.1")
        {
            context.Items["Region"] = "NL";
            context.Items["Language"] = "nl";
        }
        else
        {
            context.Items["Region"] = "DE";
            context.Items["Language"] = "de";
        }

        await _next(context);
    }
}
