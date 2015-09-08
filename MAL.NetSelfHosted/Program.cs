using System;
using System.Configuration;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.SelfHost;
using AutoMapper;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Factories;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using MAL.NetSelfHosted.Handlers;
using MAL.NetSelfHosted.Interfaces;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;

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

            //To run over https you require a ssl certificate
            //Execute the following commands to prepare the server
            //netsh http add urlacl url=https://+:port/ user=Everyone
            //netsh http add sslcert ipport=0.0.0.0:port certhash=certThumbprint appid={50e6f7c6-6ca0-4477-b3c7-178141c0df60}
            //Note: The self test will fail if the ssl certificate isn't valid - This is the expected behaviour and should NOT be changed

            var config = _host.StartsWith("https") ? new SecureHttpSelfHostConfiguration($"{_host}:{_port}") : new HttpSelfHostConfiguration($"{_host}:{_port}");

            config.Routes.MapHttpRoute("DefaultApi", "1.0/{controller}/{id}", new { id = RouteParameter.Optional });
            var server = new HttpSelfHostServer(config);

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            container.RegisterSingleton(Mapper.Engine);

            container.Register<IAnime, Anime>();
            container.Register<IAnimeOriginalJson, AnimeOriginalJson>();
            container.Register<IAnimeFactory, AnimeFactory>(Lifestyle.Singleton);
            container.Register<IAnimeRetriever, AnimeRetriever>(Lifestyle.Singleton);
            container.Register<IMappingToJson, MappingToJson>(Lifestyle.Singleton);
            container.Register<IAnimeHandler, AnimeHandler>(Lifestyle.Singleton);
            container.Register<ICacheHandler, CacheHandler>(Lifestyle.Singleton);
            container.Register<ILogWriter, LogWriter>(Lifestyle.Singleton);
            container.Register<IConsoleWriter, ConsoleWriter>(Lifestyle.Singleton);

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            server.OpenAsync().Wait();

            var consoleWriter = container.GetInstance<IConsoleWriter>();
            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("########################################################");
            Console.Write("# MAL.NET Self Hosted - Ninetail Labs".PadRight(55));
            Console.WriteLine("#");
            Console.Write($"# Version {version}".PadRight(55));
            Console.WriteLine("#");
            Console.Write($"# Running on {config.BaseAddress}".PadRight(55));
            Console.WriteLine("#");
            Console.Write($"# Startup Time - {DateTime.Now}".PadRight(55));
            Console.WriteLine("#");
            Console.WriteLine("########################################################");
            Console.WriteLine("");
            consoleWriter.WriteAsLineEnd("Running self test...", ConsoleColor.DarkYellow);

            try
            {
                var client = new HttpClient();
                var response = client.GetAsync($"{_host}:{_port}" + "/1.0/Anime").Result;
                var answer = response.Content.ReadAsStringAsync().Result;
                var result = answer == "\"Call with an ID to get an anime value\"";
                consoleWriter.WriteAsLineEnd($"Self test passed: ", ConsoleColor.DarkYellow);
                consoleWriter.WriteAsLineEnd(result.ToString(), result ? ConsoleColor.Green : ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                consoleWriter.WriteInline($"Self test passed: ", ConsoleColor.DarkYellow);
                consoleWriter.WriteAsLineEnd("False", ConsoleColor.Red);
                Console.WriteLine("");
                consoleWriter.WriteInline($"The following error occured:\r\n", ConsoleColor.Gray);
                consoleWriter.WriteAsLineEnd($"{ex}", ConsoleColor.Red);
            }


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
