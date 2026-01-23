
using Manhwa.Application;
using Manhwa.Application.Common.Extensions;
using Manhwa.Infrastructure;
using Manhwa.Infrastructure.FileStorage;
using Manhwa.Infrastructure.Realtime.Hubs;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;

namespace Manhwa.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            UrlHelper.BaseUrl = builder.Configuration["ApiBaseUrl"];
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowTestClient", policy => {
                    policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500") 
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); 
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
     
            app.UseCors("AllowTestClient");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<NotificationHub>("/hubs/notifications");
            app.MapControllers();
            app.Run();
        }
    }
}
