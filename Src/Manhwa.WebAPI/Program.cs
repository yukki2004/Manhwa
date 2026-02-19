
using Manhwa.Application;
using Manhwa.Application.Common.Extensions;
using Manhwa.Infrastructure;
using Manhwa.Infrastructure.FileStorage;
using Manhwa.Infrastructure.Middleware;
using Manhwa.Infrastructure.Realtime.Hubs;
using Manhwa.WebAPI.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;

namespace Manhwa.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            UrlHelper.BaseUrl = builder.Configuration["ApiBaseUrl"];
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAllWithCookies", policy => {
                    policy.SetIsOriginAllowed(origin => true) 
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); 
                });
            });

            var app = builder.Build();


            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors("AllowAllWithCookies");
            app.UseHttpsRedirection(); 
            app.UseAuthentication();
            app.UseMiddleware<IdentityMiddleware>();
            app.UseAuthorization();
            app.MapHub<NotificationHub>("/hubs/notifications");
            app.MapControllers(); 
            app.Run();
        }
    }
}
