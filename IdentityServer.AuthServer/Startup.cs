using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repositories;
using IdentityServer.AuthServer.Repositories.Abstract;
using IdentityServer.AuthServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer
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

            services.AddDbContext<CustomDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });


            services.AddScoped<ICustomUserRepository, CustomUserRepository>();

            var assembleyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = context =>
                    {
                        context.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlopt =>
                        {
                            sqlopt.MigrationsAssembly(assembleyName);
                        });
                    };
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = context =>
                    {
                        context.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlopt =>
                        {
                            sqlopt.MigrationsAssembly(assembleyName);
                        });
                    };
                })
                //.AddInMemoryApiResources(Config.GetApiResources)
                //.AddInMemoryApiScopes(Config.GetApiScopes())
                //.AddInMemoryClients(Config.GetClients())
                //.AddInMemoryIdentityResources(Config.GetIdentityResources)
                //.AddTestUsers(Config.GetUsers)
                .AddDeveloperSigningCredential()
                .AddProfileService<CustomProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
                



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
            app.UseIdentityServer();
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
