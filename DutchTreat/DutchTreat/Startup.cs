﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddIdentity<StoreUser, IdentityRole>(cfg =>
        {
            cfg.User.RequireUniqueEmail = true;
           
        }).AddEntityFrameworkStores<DutchContex>();
        services.AddDbContext<DutchContex>(cfg => { cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString")); });

        services.AddTransient<DutchSeeder>();

        services.AddScoped<IDutchRepository,DutchRepository>();
        services.AddMvc().AddJsonOptions(opt=>opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
         services.AddAutoMapper();
        services.AddTransient<IMailService, NullMailService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseStaticFiles();
        app.UseAuthentication();
           app.UseNodeModules(env);
        app.UseMvc(cfg =>
        {
            cfg.MapRoute("Default",
                "{controller}/{action}/{id?}",
                new { controller = "App", Action = "Index" });
        });


        }
  }
}
