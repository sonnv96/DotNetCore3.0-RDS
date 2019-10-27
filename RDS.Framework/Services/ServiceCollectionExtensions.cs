using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDS.Framework.Services.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Framework.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ResgiterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register service is here
            services.AddTransient<IUserService, UserService>();
            //services.AddSingleton<IOrderRepository, OrderRepository>();
            //services.AddSingleton<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
