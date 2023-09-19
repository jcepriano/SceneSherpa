using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SceneSherpa.DataAccess;
using SceneSherpa.Controllers;

namespace SceneSherpa.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        //THIS IS THE CODE TO ADD:
                        services.AddDbContext<SceneSherpaContext>(options =>
                            options.UseInMemoryDatabase("TestDatabase"));

                        services.AddControllersWithViews()
                            .AddApplicationPart(typeof(HomeController).Assembly);
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                        });
                    });
                });
    }
}