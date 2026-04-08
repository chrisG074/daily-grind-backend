using Microsoft.EntityFrameworkCore;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure the DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("localhost")));

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddAuthorization();

// Add CORS policy to allow frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:3000", "https://localhost:3001", "https://localhost:3002", "http://localhost:3000", "http://localhost:3001", "http://localhost:3002")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHealthChecks("/health");

app.UseRouting();

// Enable CORS before authentication
app.UseCors("AllowFrontend");

// Custom Middlewares for IP Detection & Role-Based Access Control
// Uncomment these when middleware is ready to use
// app.UseMiddleware<IpDetectionMiddleware>();
// app.UseMiddleware<CustomAuthMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
