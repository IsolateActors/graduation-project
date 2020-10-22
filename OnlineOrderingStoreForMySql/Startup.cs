using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineOrderingStore.Data;
using OnlineOrderingStore.security;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace OnlineOrderingStore
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
            services.AddControllersWithViews();

            services.AddDbContextPool<OnlineOrderingStoreContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DbConnection"), 
                mySqlOptions => mySqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 19), ServerType.MySql))));

            //添加cookie认证服务
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(OnlineUserAuthorizeAttribute.OnlineUserAuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Home/Error";
                    options.LoginPath = "/home/Login";//登录路径
                    options.LogoutPath = "/home/Logout";
                })
                .AddCookie(StoreUserAuthorizeAttribute.StoreUserAuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Home/Error";
                    options.LoginPath = "/home/Login";//登录路径
                    options.LogoutPath = "/home/Logout";
                })
                .AddCookie(AdminAuthorizeAttribute.AdminAuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Home/Error";
                    options.LoginPath = "/home/Login";//登录路径
                    options.LogoutPath = "/home/Logout";
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserService>();
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
            }

            //使用认证
            app.UseAuthentication();
            //使用授权
            app.UseAuthorization();


            app.UseStaticFiles();

            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{string?}");
            });
        }
    }
}
