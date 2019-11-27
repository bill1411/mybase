using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MyCore;
using MyDomain.IRepositories;
using MyCore.Repositories;
using MyApplications;
using MyAuthorManager.Models.ConfigModel;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace MyAuthorManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });

            //获取数据库连接字符串
            var sqlConnectionString = Configuration.GetConnectionString("Default");

            //添加数据上下文
            /*
             * 注意：Asp.Net Core提供的依赖注入拥有三种生命周期模式，由短到长依次为：
                Transient     ServiceProvider总是创建一个新的服务实例。
                Scoped         ServiceProvider创建的服务实例由自己保存，（同一次请求）所以同一个ServiceProvider对象提供的服务实例均是同一个对象。
                Singleton      始终是同一个实例对象
             可通过第二个参数ServiceLifetime来控制数据库上下文对象的生命周期，默认为Scoped
             */
            //services.AddDbContext<MyDbContext>(options =>options.UseNpgsql(sqlConnectionString),ServiceLifetime.Scoped);
            services.AddDbContext<MyDbContext>(options =>options.UseNpgsql(sqlConnectionString));

            //依赖注入
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAppService, UserAppService>();

            services.AddMvc();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //Session服务
            services.AddSession();
            //services.AddOptions();
            services
                .Configure<Setting>(Configuration.GetSection("Setting"));  //此处的Setting类是自己新建的，内容要和appsetting.json中Setting节点里字段对应起来方便以后访问
            //增加session过期时间配置项加载1800秒
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(double.Parse("1800"));
            });
            //添加对AutoMapper的支持(需要引用AutoMapper.Extensions.xxx)
            services.AddAutoMapper(typeof(MyMapper));

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //使用静态文件
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //Session
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });
            //初始化数据库的数据
            InitDBData.Initialize(app.ApplicationServices); 
        }
    }
}
