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

            HttpSelfHostConfiguration config;
            if (_host.StartsWith("https"))
            {
                config = new SecureHttpSelfHostConfiguration($"{_host}:{_port}");
            }
            else
            {
                config = new HttpSelfHostConfiguration($"{_host}:{_port}");
            }

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

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            server.OpenAsync().Wait();

            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("########################################################");
            Console.WriteLine("# MAL.NET Self Hosted - Ninetail Labs");
            Console.WriteLine($"# Version {version}");
            Console.WriteLine($"# Running on {config.BaseAddress}");
            Console.WriteLine($"# Startup Time - {DateTime.Now}");
            Console.WriteLine("########################################################");
            Console.Write("");
            Console.WriteLine("Running self test...");

            try
            {
                var client = new HttpClient();
                var response = client.GetAsync($"{_host}:{_port}" + "/1.0/Anime").Result;
                var answer = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"Self test passed: {answer == "\"Call with an ID to get an anime value\""}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Self test passed: False");
                Console.WriteLine($"The following error occured:\r\n{ex}");
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
