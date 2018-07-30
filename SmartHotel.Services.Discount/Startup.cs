using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartHotel.Services.Discount.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHotel.Services.Discount
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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Discount Api", Version = "v1" });
            });

            services.AddTransient<LoyaltyService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<LoyaltyService>>();
                return new LoyaltyService(Configuration["profilesve"], logger);
            });

            services.AddMvc();
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var pbase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pbase))
            {
                app.UsePathBase(pbase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var path = string.IsNullOrEmpty(pbase) || pbase == "/" ? "/" : $"{pbase}/";
                options.SwaggerEndpoint($"{path}swagger/v1/swagger.json", "Discounts Api");
            });

            app.UseResponseCompression();

            app.UseMvc();
        }
    }
}
