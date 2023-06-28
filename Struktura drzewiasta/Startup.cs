using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Struktura_drzewiasta.Context;
using Struktura_drzewiasta.Dtos;
using Struktura_drzewiasta.Models;
using Struktura_drzewiasta.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_drzewiasta
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
            services.AddControllersWithViews();

            // Sesja otrzebna do flagi dla Reverse
            services.AddSession();

            // Konfiguracja mapy mappowania
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TreeNode, TreeNodeDto>();
                config.CreateMap<TreeNodeDto, TreeNode>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            // Rejestracja serwis�w
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(mapper);

            // Rejestrowanie serwisu dla DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Rejestracja serwisu dla TreeNode (logika)
            services.AddScoped<TreeNodeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Wywo�anie inicjalizacji, �eby dodwa� warto�ci pocz�tkowe do bazy danych
            ApplicationDbContextInitializer.Initialize(dbContext);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            // Routing (kontroler)
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=TreeNode}/{action=Index}/{id?}");

            });
        }
    }
}
