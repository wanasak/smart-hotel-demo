using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartHotel.Services.Booking.Commands;
using SmartHotel.Services.Booking.Data;
using SmartHotel.Services.Booking.Data.Repositories;
using SmartHotel.Services.Booking.Domain;
using SmartHotel.Services.Booking.Queries;
using SmartHotel.Services.Booking.Settings;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHotel.Services.Booking
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(env.ContentRootPath)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<UserBookingQuery>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<BookingRepository>();
            services.AddTransient<UserBookingQuery>();
            services.AddTransient<CreateBookingCommand>();

            services.Configure<AppSettings>(Configuration);
            services.Configure<SecuritySettings>(Configuration.GetSection("b2c"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Booking Api", Version = "v1" });
            });

            services.ConfigureSwaggerGen(swaggerGen =>
            {
                var b2cConfig = Configuration.GetSection("b2c");
                var authority = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0", b2cConfig["Tenant"]);
                swaggerGen.AddSecurityDefinition("Swagger", new OAuth2Scheme
                {
                    AuthorizationUrl = authority + "/authorize",
                    Flow = "implicit",
                    TokenUrl = authority + "/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid","User offline" },
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOpt =>
                {
                    var b2cConfig = Configuration.GetSection("b2c");
                    jwtOpt.Authority = string.Format("https://login.microsoftonline.com/tfp/{0}/{1}/v2.0/", b2cConfig["tenant"], b2cConfig["policy"]);

                    jwtOpt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = b2cConfig["audiences"].Split(',')
                    };
                    jwtOpt.Events = new JwtBearerEvents();
                }); ;

            services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var pbase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pbase))
                app.UsePathBase(pbase);

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseByPassAuth();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var b2cConfig = Configuration.GetSection("b2c");
                var path = string.IsNullOrEmpty(pbase) || pbase == "/" ? "/" : $"{pbase}/";
                c.InjectOnCompleteJavaScript($"{path}swagger-ui-b2c.js");
                c.SwaggerEndpoint($"{path}swagger/v1/swagger.json", "Bookings Api");
                c.ConfigureOAuth2(b2cConfig["ClientId"], "y", "z", "z", "",
                    new
                    {
                        p = "B2C_1_SignUpInPolicy",
                        prompt = "login",
                        nonce = "defaultNonce"
                    });
            });

            app.UseMvc();
        }
    }
}
