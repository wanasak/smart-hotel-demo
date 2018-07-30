using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SmartHotel.Services.Notification.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHotel.Services.Notification
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<NotificationService>();
            
            services.AddSwaggerGen(setup => {
                setup.SwaggerDoc("v1", new Info { Title = "Notification API", Version = "v1" });
            });
            services.ConfigureSwaggerGen(setup => {
                var b2cConfig = Configuration.GetSection("b2c");
                var authority = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0", b2cConfig["Tenant"]);
                setup.AddSecurityDefinition("Swagger", new OAuth2Scheme
                {
                    AuthorizationUrl = authority + "/authorize",
                    Flow = "implicit",
                    TokenUrl = authority + "/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "User offline"}
                    }
                });
            }); 

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOpt =>
                {
                    var b2cConfig = Configuration.GetSection("b2c");
                    jwtOpt.Authority = string.Format("https://login.microsoftonline.com/tfp/{0}/{1}/v2.0/", b2cConfig["tenant"], b2cConfig["policy"]);

                    jwtOpt.TokenValidationParameters = new TokenValidationParameters {
                        ValidAudiences = b2cConfig["audiences"].Split(',')
                    };
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

            app.UseCors(builder => {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseByPassAuth();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                var b2cConfig = Configuration.GetSection("b2c");
                var path = string.IsNullOrEmpty(pbase) || pbase == "/" ? "/" : $"{pbase}/";
                options.InjectOnCompleteJavaScript($"{path}swagger-ui-b2c.js");
                options.SwaggerEndpoint($"{path}swagger/v1/swagger.json", "Notification API");
                options.ConfigureOAuth2(b2cConfig["ClientId"], "y", "z", "z", "",
                    new {
                        p = "B2C_1_SignUpInPolicy",
                        prompt = "login",
                        nonce = "defaultNonce"
                    });
            });

            app.UseResponseCompression();

            app.UseMvc();
        }
    }
}
