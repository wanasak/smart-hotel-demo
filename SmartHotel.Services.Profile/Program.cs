using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHotel.Services.Profile.Data;
using SmartHotel.Services.Profile.Data.Seed;
using SmartHotel.Services.Profile.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHotel.Services.Profile
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<ProfileDbContext>((context, services) =>
                {
                    var db = services.GetService<ProfileDbContext>();
                      ProfileDbContextSeed.Seed(db);
                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
