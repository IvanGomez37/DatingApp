using API.Data;
using API.Extensions;
using API.Middlewares;
using Microsoft.EntityFrameworkCore;
//dotnet watch --no-hot-reload 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

//Configure the hhtp request pipeline
app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins(
        "http://localhost:4200", 
        "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsersAsync(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error ocurred during migration/seeding");
}

app.Run();
