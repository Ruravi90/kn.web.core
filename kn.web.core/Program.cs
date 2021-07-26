using kn.web.core._Socket;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace kn.web.core
{
    public class Program
    {
        static string url;
        public  static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json").Build();

            Program.url = config.GetSection("ConnectionStrings:Default").Value;

            new Thread(new ThreadStart(Program.socket)).Start();
            
            CreateHostBuilder(args).Build().Run();
        }

        static void socket() {
            new SocketListener(Program.url).StartListening();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls(new[] { "http://*:8081" });
                });
    }
}
