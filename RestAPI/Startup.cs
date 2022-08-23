using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RestAPI.CoolerDbContext;
using RestAPI.Manager;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RestAPI
{    
    public class Startup
    {
        public const string AllowAllPolicyName = "AllowAll";
        public const string AllowOnlyGetMethodPolicyName = "AllowOnlyGetMethod";
        public const string AllowOnlyZealandOriginPolicyName = "AllowOnlyZealandOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Having a policy that allows all
            services.AddCors(options => options.AddPolicy(AllowAllPolicyName,
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            //A policy that only allows GET, but everything else
            //We don't use this in this application, this is just an example
            services.AddCors(options => options.AddPolicy(AllowOnlyGetMethodPolicyName,
                builder => builder.AllowAnyOrigin()
                .WithMethods("GET")
                .AllowAnyHeader()));

            //A policy that only allow requests coming from zealand.dk
            services.AddCors(options => options.AddPolicy(AllowOnlyZealandOriginPolicyName,
                builder => builder.WithOrigins("https://zealand.dk")
                .AllowAnyMethod()
                .AllowAnyHeader()));

            //services.AddTransient<CoolerDbManager>();

            services.AddDbContext<CoolerContext>(options =>
            {
                options.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = CoolerDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });

                //add-migration
                //Name: intialize database
                //update-database

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
