using FluentValidation;
using Manhwa.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                // Đăng ký Behavior này để nó chạy trước mọi Handler
                cfg.AddBehavior(typeof(IPipelineBehavior<, >), typeof(ValidationBehavior<, >));
            });
            services.AddHttpClient();


            return services;
        }
    }
}
