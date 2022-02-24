using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Phonebook.Data;
using FluentValidation.AspNetCore;
using FluentValidation;
using Phonebook.Validators;
using Phonebook.Models;
using Phonebook.Data.Repositories.Base;
using Phonebook.Data.Repositories;
using Phonebook.Services.Base;
using Phonebook.Services;
using StackExchange.Redis;

namespace Phonebook
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
            services.AddDbContext<ApplicationDbContext>(opts => opts.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddFluentValidation();
            services.AddTransient<IValidator<Contact>, ContactValidator>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddDistributedRedisCache(options =>
             {
                 options.InstanceName = Configuration["Redis:InstanceName"];
                 options.ConfigurationOptions = new ConfigurationOptions()
                 {
                     EndPoints = { { Configuration.GetConnectionString("Redis"), int.Parse(Configuration["Redis:Port"]) } },
                     ConnectRetry = 2,
                     ReconnectRetryPolicy = new LinearRetry(10),
                     AbortOnConnectFail = false,
                     ConnectTimeout = 5000,
                     SyncTimeout = 1000,
                     DefaultDatabase = 0,
                 };
             });
            services.AddScoped<ICacheService<Contact>, ContactsCacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
