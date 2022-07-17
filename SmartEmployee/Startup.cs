using DAL;
using Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartEmployee.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEmployee
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this._environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnection = new DatabaseConnection { ConnectionString = Configuration.GetConnectionString("DefaultConnection") };

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartEmployee", Version = "v1" });
            });

            services.AddSingleton(dbConnection);
            services.AddScoped<VendorSQL>();
            services.AddScoped<CompanySQL>();
            services.AddScoped<PurchaseDetailSQL>();
            services.AddScoped<PurchaseMainSQL>();
            services.AddScoped<UsersSQL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartEmployee v1"));
            }

            lifetime.ApplicationStarted.Register(OnApplicationStarted);

            app.UseStaticFiles();
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void OnApplicationStarted()
        {
            try
            {
                var purchaseMainPath = Path.Combine(_environment.WebRootPath, PurchaseMainController._purchaseMainFileUploadFolder);

                if (!Directory.Exists(purchaseMainPath))
                {
                    Directory.CreateDirectory(purchaseMainPath);
                }

                var tempFilePath = Path.Combine(_environment.WebRootPath, PurchaseMainController._tempFile);

                if (Directory.Exists(tempFilePath))
                {
                    Directory.Delete(tempFilePath, true);
                }

                Directory.CreateDirectory(tempFilePath);
            }
            catch
            {
                //Do nothing
            }
        }
    }
}
