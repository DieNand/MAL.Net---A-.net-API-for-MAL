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
            //netsh http add sslcert ipport = 0.0.0.0:port certhash = certThumbprint appid ={ 50e6f7c6 - 6ca0 - 4477 - b3c7 - 178141c0df60}
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
