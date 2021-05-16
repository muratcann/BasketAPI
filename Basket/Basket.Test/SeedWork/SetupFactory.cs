using Basket.Api.Data;
using Basket.Api.Repositories;
using Basket.Api.Repositories.EntityFramework;
using Basket.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Test.SeedWork
{
    public class SetupFactory
    {
        public static IServiceProvider Create()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            services.AddDbContext<EFDbContext>(opt => opt.UseInMemoryDatabase("TestProductDb"));
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IRepository, EFRepository>();
            services.AddScoped<IRepositoryReadOnly, EFRepositoryReadOnly>();

            services.Add(
                ServiceDescriptor.Singleton<IBasketContext>
                (
                    new BasketContext(configuration)
                ));
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["CacheSettings:ConnectionString"];
            });

            var provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
