using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SanFu.AutoMapping;
using SanFu.Commons;
using SanFu.Commons.AppSettings;
using SanFu.DataSource;
using SanFu.PassPort.AppSetts;
using SanFu.PassPort.IdentityConfig;

namespace SanFu.PassPort
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddJsonFile("Configs/autofac.json")
            .AddEnvironmentVariables();
            Configuration = builder.Build();

            //log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(Directory.GetCurrentDirectory() + @"/Configs/log4net.config"));
            ContainerRepository.Log4NetRepository = logRepository;
        }

        public IConfiguration Configuration { get; }

        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<EfDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//配置sqlserver


            /* 配置文件注入方式 */
            services.Configure<ApiAccessSettings>(Configuration.GetSection("ApiAccessSettings"));
            services.AddOptions();


            var config = Configuration.GetSection("IdentityServer").Get<IdentityServer>();
            services.AddIdentityServer()
            .AddDeveloperSigningCredential(filename: "tempkey.rsa")
            .AddInMemoryIdentityResources(Configs.GetIdentityResources())
            .AddInMemoryApiResources(Configs.GetApiResources(config))
            .AddInMemoryClients(Configs.GetClients(config))
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddProfileService<ProfileService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper(typeof(IProfile));

            #region autofac
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var module = new ConfigurationModule(Configuration);
            builder.Register(c => new AopInterceptor());
            builder.RegisterModule(module);
            this.Container = builder.Build();
            #endregion

            return new AutofacServiceProvider(this.Container); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
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
            app.UseStaticFiles().UseIdentityServer();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles().UseIdentityServer();
            app.UseCookiePolicy();

            //autofac
            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }
    }
}
