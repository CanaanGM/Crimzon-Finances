using Application.Core;
using Application.Purchases;

using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// to be wxtracted into extension class 
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer"]);
});

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
}));

builder.Services.AddMediatR(typeof(List.Handler).Assembly);

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

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



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy"); 
app.UseAuthorization();

app.MapControllers();

app.Run();
