using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Data.Repository;
using DutchTreat.Data.UoW;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Infra - Identity
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DutchContext>();
            
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };
                });

            // Infra - Data
            services.AddDbContext<DutchContext>(cfg =>
                cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"))
            );
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<Order>, EFRepository<Order, DutchContext>>();
            services.AddScoped<IRepository<Product>, EFRepository<Product, DutchContext>>();

            services.AddTransient<DutchSeeder>();
            
            // Infra - Services
            services.AddScoped<IMailService, NullMailService>();

            services.AddAutoMapper();

            services.AddMvc()
                    .AddJsonOptions(opt =>
                        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Check if env.IsEnviroment("Development") -- ASPNETCORE_ENVIRONMENT == 'Development'
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("<html><body><h1>Hello, World!</h1></body></html>");
            //});
            //app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseNodeModules(env);

            app.UseAuthentication();

            app.UseMvc(configuration =>
            {
                configuration.MapRoute("Default",
                    "/{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });
            });
        }
    }
}
