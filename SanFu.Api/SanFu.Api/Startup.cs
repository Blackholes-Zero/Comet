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
using IdentityServer4.AccessTokenValidation;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using NetCore.Framework;
using Newtonsoft.Json;
using SanFu.Api.Filter;
using SanFu.AutoMapping;
using SanFu.Commons;
using SanFu.Commons.AppSettings;
using SanFu.DataSource;
using Swashbuckle.AspNetCore.Swagger;

namespace SanFu.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //Configuration = configuration;
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
            /*ef*/
            services.AddDbContext<EfDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//配置sqlserver

            /* 配置文件注入方式 */
            services.Configure<ApiAccessSettings>(Configuration.GetSection("ApiAccessSettings"));
            services.AddOptions();


            #region 跨域
            var urls = Configuration.GetSection("CoresSettings:Urls").Value.Split(',');
            services.AddCors(options =>
            options.AddPolicy("CoresDomain", blder =>
             blder.WithOrigins(urls)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowAnyOrigin()
             .AllowCredentials())
             );
            #endregion 跨域

            #region identityServer4
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetSection("IdentityServerSettings:Authority").Value;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Configuration.GetSection("IdentityServerSettings:ApiName").Value;
                });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Netcore.Api接口文档",
                    Description = "RESTful API for Netcore.Api",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Alvin_Su", Email = "939391793@qq.com", Url = "" }
                });

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
                c.IgnoreObsoleteActions();
                //identityServer4
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "password",
                    //AuthorizationUrl = "http://localhost:5006",
                    TokenUrl = string.Concat(Configuration.GetSection("IdentityServerSettings:Authority").Value.TrimEnd('/')
                    , Configuration.GetSection("IdentityServerSettings:TokenUrl").Value),//  "http://auth.handnear.com/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "Api", "secret" }
                    },
                });
                c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
            });
            #endregion

            services.AddMvc(options => {
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<ApiActionFilter>();
                options.Filters.Add(new ApiAuthorizationFilter());
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //默认返回json为小写，设置后不会变换model字段的大小写
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.FloatFormatHandling = FloatFormatHandling.String;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new LongJsonConverter()); //long unlong丢失精度处理，转换为字符串
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //automapper
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

            // 注入，文件路径更换
             var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(filePath),
                RequestPath = new PathString("/src")
            });

            app.UseCors("CoresDomain");

            //IdentityServer4
            app.UseAuthentication();
            app.Use((context, next) =>
            {
                var user = context.User;
                context.Response.StatusCode = user.Identity.IsAuthenticated ? 200 : 401;
                return next.Invoke();
            });

            //Swagger
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore.Api API V1");
                //c.ShowRequestHeaders();
            });


            app.UseMvc();

            //autofac
            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }
    }
}
