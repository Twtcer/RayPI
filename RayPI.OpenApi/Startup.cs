﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//
using RayPI.ConfigService;
using RayPI.SwaggerService;
using RayPI.CorsService;
using RayPI.OpenApi.Filters;
using RayPI.Business.Di;
using RayPI.Repository.EFRepository.Di;
using RayPI.Infrastructure.Auth.Di;
using RayPI.Infrastructure.Auth.Operate;

namespace RayPI.OpenApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _config = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //注册MVC
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//设置时间格式
                });

            //注册配置管理服务
            services.AddConfigService(_config);

            //注册Swagger
            services.AddSwaggerService();

            //注册授权认证
            services.AddAuthService(_config);

            //注册Cors跨域
            services.AddCorsService();

            //注册http上下文访问器
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //注册IOperateInfo
            services.AddScoped<IOperateInfo, OperateInfo>();

            //注册仓储
            services.AddRepository(_config);

            //注册业务逻辑
            services.AddBusiness();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();//自定义异常处理中间件

            app.UseAuthService();

            app.UseMvc();

            app.UseSwaggerService();

            app.UseStaticFiles();//用于访问wwwroot下的文件
        }

    }
}