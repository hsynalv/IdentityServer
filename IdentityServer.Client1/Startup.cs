using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Client1
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
            services.AddHttpContextAccessor();
            services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = "Cookies";
                //opt.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", opt =>
            {
                opt.LoginPath = "/Login/Index";
            });


            //.AddCookie("Cookies").AddOpenIdConnect("oidc", opt =>
            //{
            //    opt.SignInScheme = "Cookies";
            //    opt.Authority = "https://localhost:5001";
            //    opt.ClientId = "Client1-Mvc";
            //    opt.ClientSecret = "Secret";
            //    opt.ResponseType = "code id_token";
            //    opt.GetClaimsFromUserInfoEndpoint = true;
            //    opt.SaveTokens = true;
            //    opt.Scope.Add("api1.read");
            //    opt.Scope.Add("offline_access");
            //    opt.Scope.Add("CountryAndCity");
            //    opt.Scope.Add("email");

            //    opt.ClaimActions.MapUniqueJsonKey("Country", "Country");
            //    opt.ClaimActions.MapUniqueJsonKey("City", "City");

            //});

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
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
