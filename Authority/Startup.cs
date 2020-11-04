using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authority.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Authority.Services;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using IdentityServer4;
using System.Linq;
using System.Collections.Generic;
using IdentityServer4.Models;
using System.Threading.Tasks;
using IdentityModel;

namespace Authority
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.ConsentCookie.Name = "AuthorityConsent";
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.ClaimsIdentity.UserIdClaimType = "sub";
                options.ClaimsIdentity.RoleClaimType = "role";
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsLogged", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("Administrátor");
                });
                options.AddPolicy(Constants.LocalScopeName, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // custom requirements
                });
                options.AddPolicy("ApiAdmin", policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    //policy.RequireRole("Administrátor");
                    policy.RequireClaim("admin","1");
                });
            });

            services.AddAuthentication()
                .AddCookie("Cookies")
                .AddMicrosoftAccount(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                    options.SaveTokens = true;
                });
                /*.AddLocalApi(options => 
                {
                    options.ExpectedScope = Constants.LocalScopeName;
                })*/
            services.AddLocalApiAuthentication();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Login";
                options.LogoutPath = $"/Logout";
                options.AccessDeniedPath = $"/AccessDenied";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });

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

            List<ApiResource> ApiResources = new List<ApiResource>();
            foreach (var ar in Configuration.GetSection("ApiResources").GetChildren())
            {
                ApiResources.Add(new ApiResource(ar.GetValue<string>("Name"), ar.GetValue<string>("DisplayName")) 
                { 
                    UserClaims = { JwtClaimTypes.Audience},
                    Scopes = new List<string> { ar.GetValue<string>("Name") }
                }
                );
            };
            List<ApiScope> ApiScopes = new List<ApiScope>();
            foreach (var ar in Configuration.GetSection("ApiResources").GetChildren())
            {
                ApiScopes.Add(new ApiScope(ar.GetValue<string>("Name"), ar.GetValue<string>("DisplayName")));
            };

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Login";
                options.UserInteraction.ConsentUrl = "/Consent";
                options.UserInteraction.LogoutUrl = "/Logout";
                options.UserInteraction.ErrorUrl = "/Error";
            })
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 90;
                })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryApiResources(ApiResources)
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService<ApplicationUser>>()
                .AddInMemoryApiScopes(ApiScopes)
                //.AddInMemoryClients(Config.GetClients())
                ;

            services.AddScoped<EmailSender>();
            services.AddScoped<RazorViewToStringRenderer>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseCors("default");
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
             //   endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(name: "login",
                    pattern: "Login",
                    defaults: new { controller = "Account", action = "Login" }
                );
                endpoints.MapControllerRoute(name: "logout",
                    pattern: "Logout",
                    defaults: new { controller = "Account", action = "Logout" }
                );
                endpoints.MapControllerRoute(name: "accessdenied",
                    pattern: "AccessDenied",
                    defaults: new { controller = "Home", action = "AccessDenied" }
                );
                endpoints.MapControllerRoute(name: "error",
                    pattern: "Error",
                    defaults: new { controller = "Home", action = "Error" }
                );
                endpoints.MapControllerRoute(name: "register",
                    pattern: "Register",
                    defaults: new { controller = "Account", action = "Register" }
                );
                endpoints.MapControllerRoute("default", "{area}","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
