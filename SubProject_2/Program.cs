using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SubProject_2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO: undersøg frameworkets inner workings.   BuildWebHost(args).Run();   og  WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls("http://localhost:5001/").Build();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5001/")
                .Build();
    }
}
