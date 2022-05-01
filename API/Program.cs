using API.Extensions;
using API.Middleware;

using Application.Purchases;

using Domain;

using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));

}).AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<Create>();
});

//! extension custom classes
ApplicationServiceExtensions.AddApplicationServices(builder.Services, builder.Configuration);
IdentityServiceExtension.AddIdentityServices(builder.Services, builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();