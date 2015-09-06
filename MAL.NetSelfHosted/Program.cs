using System;
using System.Configuration;
using System.Net.Http;
using System.Reflection;
using Microsoft.Owin.Hosting;

namespace MAL.NetSelfHosted
{
    class Program
    {
        #region Variables

        private static string _host;
        private static string _port;

        #endregion

        static void Main(string[] args)
        {
            ReadSettings();

            WebApp.Start<Startup>($"{_host}:{_port}");

            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("########################################################");
            Console.WriteLine("# MAL.NET Self Hosted - Ninetail Labs");
            Console.WriteLine($"# Version {version}");
            Console.WriteLine($"# Running on {_host}:{_port}");
            Console.WriteLine($"# Startup Time - {DateTime.Now}");
            Console.WriteLine("########################################################");
            Console.Write("");
            Console.WriteLine("Running self test...");

            var client = new HttpClient();
            var response = client.GetAsync($"{_host}:{_port}" + "/1.0/Anime").Result;
            var answer = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Self test passed: {answer == "\"Call with an ID to get an anime value\""}");
            
            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to exit...");
            Console.WriteLine("");

            Console.ReadLine();
        }

        private static void ReadSettings()
        {
            _host = ConfigurationManager.AppSettings["Host"];
            _port = ConfigurationManager.AppSettings["Port"];
        }
    }
}
