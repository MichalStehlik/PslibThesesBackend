using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using PslibThesesBackend.Models;
using Serilog;

namespace PslibThesesBackend
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
            services.AddDbContext<ThesesContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );
            services.AddControllers();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Logged", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("Administrator", policy =>
                {
                    //policy.RequireClaim("theses_admin", "1");
                    policy.RequireAssertion(context => (context.User.HasClaim(c => c.Type == "theses_admin" && c.Value == "1") && context.User.HasClaim(c => c.Type == "theses_robot" && c.Value == "1")));
                    //policy.RequireRole("Administrátor");
                    //policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("Author", policy =>
                {
                    policy.RequireClaim("theses_author","1");
                });
                options.AddPolicy("Evaluator", policy =>
                {
                    policy.RequireClaim("theses_evaluator","1");
                });
                options.AddPolicy("Manager", policy =>
                {
                    policy.RequireClaim("theses_manager", "1");
                });
            });
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Authority = Configuration["Authority:Server"];
                options.RequireHttpsMetadata = true;
                options.Audience = "ThesesApi";
            }
            );

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
