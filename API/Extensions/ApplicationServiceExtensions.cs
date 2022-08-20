using Application.Core;
using Application.Interfaces;
using Application.Purchases;

using Infrastucture.Security;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Financing App",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact { Email = "canaand1@gmail.com", Name = "Canaan" },
                    Description = "Self Financing app to be used locally...for the time being",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            // to be wxtracted into extension class 
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config["ConnectionStrings:SqlServer"], conf =>
                {
                    conf.EnableRetryOnFailure(); 
                });
               
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));

            services.AddMediatR(typeof(List.Handler).Assembly);

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddScoped<IUserAccessor, UserAccessor>();

            return services;

        }
    }
}
