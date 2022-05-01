using Application.Core;
using Application.Interfaces;
using Application.Purchases;

using Infrastucture.Security;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


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
