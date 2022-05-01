using API.Extensions;
using API.Middleware;

using Application.Purchases;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;

using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<Create>();
});


//! extension custom class
ApplicationServiceExtensions.AddApplicationServices(builder.Services, builder.Configuration);

var app = builder.Build();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Migratoin Failed!");
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();