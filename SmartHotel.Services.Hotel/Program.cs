using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHotel.Services.Hotel.Data;
using SmartHotel.Services.Hotel.Data.Seed;
using SmartHotel.Services.Hotel.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHotel.Services.Hotel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<HotelDbContext>((context, services) =>
                {
                    var db = services.GetService<HotelDbContext>();
                    HotelDbContextSeed.Seed(db);
                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
