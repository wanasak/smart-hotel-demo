using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHotel.Services.Task.Data;
using SmartHotel.Services.Task.Data.Seeder;
using SmartHotel.Services.Task.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHotel.Services.Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<TaskDbContext>((context, services) =>
                {
                    var db = services.GetService<TaskDbContext>();
                    TaskSeeder.Seed(db);
                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
