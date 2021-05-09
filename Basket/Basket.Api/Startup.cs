using Basket.Model.Dto;
using Basket.Model.EntityFramework;
using Basket.Model.Repositories;
using Basket.Model.Repositories.Implementation;
using Basket.Service.Interface;
using Basket.Service.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EFDbContext>(opt => opt.UseInMemoryDatabase("TestProductDb"));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IRepository, EFRepository>();
            services.AddScoped<IRepositoryReadOnly, EFRepositoryReadOnly>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                var context = serviceProvider.GetService<EFDbContext>();
                AddTestData(context);

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddTestData(EFDbContext context)
        {
            IList<Product> products = new List<Product>();
            products.Add(new Product { Id = 1, Name = "Ti��rt", Price = 40.00M, Photo = "", Description = "K�rm�z� Ti��rt", Stock = 100, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 1 });
            products.Add(new Product { Id = 2, Name = "G�mlek", Price = 70.00M, Photo = "", Description = "Beyaz G�mlek", Stock = 50, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 1 });
            products.Add(new Product { Id = 3, Name = "Kazak", Price = 60.00M, Photo = "", Description = "�nce Kazak", Stock = 70, OrderMinimumQuantity = 10, OrderMaximumQuantity = 100, Status = 1 });
            products.Add(new Product { Id = 4, Name = "�orap", Price = 15.00M, Photo = "", Description = "Soket �orap", Stock = 60, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 0 });
            products.Add(new Product { Id = 5, Name = "Ayakkab�", Price = 120.00M, Photo = "", Description = "Spor Ayakkab�", Stock = 0, OrderMinimumQuantity = 1, OrderMaximumQuantity = 100, Status = 1 });
            context.Products.AddRange(products);

            IList<User> users = new List<User>();
            users.Add(new User { Id = 1, Name = "Ahmet", Email = "ahmet@gmail.com", Status = 1 });
            users.Add(new User { Id = 2, Name = "Ay�e", Email = "ayse@gmail.com", Status = 0 });
            context.Users.AddRange(users);

            //IList<BasketItem> basketItems = new List<BasketItem>();
            //basketItems.Add(new BasketItem { Id = 1, ProductId = 1, Quantity = 2, UserId = 1 });
            //basketItems.Add(new BasketItem { Id = 2, ProductId = 5, Quantity = 1, UserId = 1 });
            //context.BasketItems.AddRange(basketItems);

            context.SaveChanges();
        }
    }
}
