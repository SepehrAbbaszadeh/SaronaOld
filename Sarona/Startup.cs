﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sarona.Models;

namespace Sarona
{

    public class Startup
    {
        public Startup(IConfiguration config) => Configuration = config;
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<SaronaContext>(options => options.UseSqlServer(conString));
            services.AddTransient<SaronaRepository>();

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(conString));

            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();




            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(rt =>
            {


                rt.MapRoute(
                    name: "Element",
                    template: "Network/{district:length(2)}/{exchange}/{ne}/{action}",
                    defaults: new { controller = "Network", action = "Specifications" });
                rt.MapRoute(
                    name: "Exchange",
                    template: "Network/{district:length(2)}/{exchange}",
                    defaults: new { controller = "Network", action = "Exchange" });
                rt.MapRoute(
                    name: "District",
                    template: "Network/{district:length(2)}",
                    defaults: new { controller = "Network", action = "District", district = Area.A2 });

                rt.MapRoute(null, "{controller=Home}/{action=Index}");
            });
        }
    }
}
