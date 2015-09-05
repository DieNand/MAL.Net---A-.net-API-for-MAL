using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.Owin.Hosting;

namespace MAL.NetSelfHosted
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "http://localhost:7331";

            WebApp.Start<Startup>(host);

            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("########################################################");
            Console.WriteLine("# MAL.NET Self Hosted - Ninetail Labs");
            Console.WriteLine($"# Version {version}");
            Console.WriteLine($"# Startup Time - {DateTime.Now}");
            Console.WriteLine("########################################################");
            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to exit...");
            Console.WriteLine("");

            Console.ReadLine();
        }
    }
}
