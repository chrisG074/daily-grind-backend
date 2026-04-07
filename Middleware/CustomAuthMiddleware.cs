using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class CustomAuthMiddleware
{
    private readonly RequestDelegate _next;

    public CustomAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Mock token check/Role based check using a header
        if (context.Request.Headers.TryGetValue("x-user-role", out var roleStr))
        {
            var claims = new[] 
            { 
                new Claim(ClaimTypes.Name, "User"), 
                new Claim(ClaimTypes.Role, roleStr.ToString()) 
            };
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }
}
