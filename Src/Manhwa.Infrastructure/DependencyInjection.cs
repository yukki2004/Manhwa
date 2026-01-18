using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhwa.Application.Common.Interfaces;
using Manhwa.Domain.Repositories;
using Manhwa.Infrastructure.Identity;
using Manhwa.Infrastructure.Messaging.Consumers;
using Manhwa.Infrastructure.Persistence;
using Manhwa.Infrastructure.Persistence.Repositories;
using Manhwa.Infrastructure.Security;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
namespace Manhwa.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // cấu hình postgreSQL
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<IIdentityService, JwtService>();

            // Cấu hình Authentication & JWT
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                // Đọc Token từ Cookie cho hệ thống Manhwa
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["accessToken"];
                        return Task.CompletedTask;
                    }
                };
            });
            // cấu hình RabbitMQ
            var host = configuration["RabbitMqSettings:Host"] ?? "localhost";
            var user = configuration["RabbitMqSettings:UserName"] ?? "guest";
            var pass = configuration["RabbitMqSettings:Password"] ?? "guest";

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserLogConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });

                    cfg.ReceiveEndpoint("user-log-queue", e =>
                    {
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        e.ConfigureConsumer<UserLogConsumer>(context);
                    });
                });
            });
            // cấu hình các interface
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLogRepository, UserLogRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
            
        }
    }
}
