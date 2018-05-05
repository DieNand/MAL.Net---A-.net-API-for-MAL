using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.SelfHost;
using AutoMapper;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Factories;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using MAL.NetSelfHosted.Classes;
using MAL.NetSelfHosted.Enumerations;
using MAL.NetSelfHosted.Handlers;
using MAL.NetSelfHosted.Interfaces;
using Serilog;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;
using Swashbuckle.Application;

namespace MAL.NetSelfHosted
{
    class Program
    {
        #region Variables

        private const string EmptyVersion = "0.0.0.0";
        private static string _host;
        private static string _port;

        #endregion

        static void Main(string[] args)
        {
            ReadSettings();
            var config = ConfigureRoutes();
            ConfigureSwagger(config);
            var server = new HttpSelfHostServer(config);
            var container = ConfigureContainer(config);
            SetupLogging();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            server.OpenAsync().Wait();

            Console.WriteLine("########################################################");
            Console.Write("# MAL.NET Self Hosted - Ninetail Labs".PadRight(55));
            Console.WriteLine("#");
            Console.Write($"# Version {GetGitInformation()}".PadRight(55));
            Console.WriteLine("#");
            Console.Write($"# Running on {config.BaseAddress}".PadRight(55));
            Console.WriteLine("#");
            Console.Write($"# Startup Time - {DateTime.Now}".PadRight(55));
            Console.WriteLine("#");
            Console.WriteLine("########################################################");
            Console.WriteLine("");
            Log.Information("Executing self test...");

            try
            {
                var client = new HttpClient();
                var response = client.GetAsync($"{_host}:{_port}" + "/1.0/Anime").Result;
                var answer = response.Content.ReadAsStringAsync().Result;
                var result = answer == "\"Call with an ID to get an anime value\"";
                Log.Information("Self test passed: {Result}", result);
              }
            catch (Exception ex)
            {
                Log.Error(ex, "Self test failed");
            }


            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to exit...");
            Console.WriteLine("");

            Console.ReadLine();
        }

        private static Container ConfigureContainer(HttpSelfHostConfiguration config)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            container.RegisterSingleton(Mapper.Engine);

            container.Register<IAnimeFactory, AnimeFactory>(Lifestyle.Singleton);
            container.Register<IAnimeRetriever, AnimeRetriever>(Lifestyle.Singleton);
            container.Register<IMappingToJson, MappingToJson>(Lifestyle.Singleton);
            container.Register<IAnimeHandler, AnimeHandler>(Lifestyle.Singleton);
            container.Register<ICacheHandler, CacheHandler>(Lifestyle.Singleton);
            container.Register<IWebHttpWebRequestFactory, WebHttpWebRequestFactory>(Lifestyle.Singleton);
            container.Register<IWebHttpWebRequest, WebHttpWebRequest>();
            container.Register<IAnimeListRetriever, AnimeListRetriever>();
            container.Register<IDataPush, DataPush>();
            container.Register<ICredentialVerification, CredentialVerification>();
            container.Register<ICharacterFactory, CharacterFactory>(Lifestyle.Singleton);

            container.Register<IAnime, Anime>();
            container.Register<IAnimeDetails, AnimeDetails>();
            container.Register<IAnimeDetailsJson, AnimeDetailsJson>();
            container.Register<IAnimeDetailsXml, AnimeDetailsXml>();
            container.Register<IAnimeOriginalJson, AnimeOriginalJson>();
            container.Register<IListAnime, ListAnime>();
            container.Register<IListAnimeJson, ListAnimeJson>();
            container.Register<IListAnimeXml, ListAnimeXml>();
            container.Register<IMyAnimeList, MyAnimeList>();
            container.Register<IMyAnimeListJson, MyAnimeListJson>();
            container.Register<IMyInfo, MyInfo>();
            container.Register<ILoginData, LoginData>();
            container.Register<ISeiyuuInformation, SeiyuuInformation>();
            container.Register<ICharacterInformation, CharacterInformation>();
            container.Register<IAnimeographyJson, AnimeographyJson>();
            container.Register<ISeasonData, SeasonData>();
            container.Register<ISeasonRetriever, SeasonRetriever>();
            container.Register<ISeasonFactory, SeasonFactory>();
            container.Register<ISeasonLookup, SeasonLookup>();
            container.Register<IUrlHelper, NetLogic.Helpers.UrlHelper>(Lifestyle.Singleton);
            container.RegisterWebApiControllers(config);

            container.Verify();
            return container;
        }

        private static void ConfigureSwagger(HttpSelfHostConfiguration config)
        {
            //Configure swagger
            config.EnableSwagger((c) =>
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);

                c.SingleApiVersion("v1", "MAL.Net - A .net API for MAL");
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(commentsFile);
            }).EnableSwaggerUi();
        }

        private static HttpSelfHostConfiguration ConfigureRoutes()
        {
            //To run over https you require a ssl certificate
            //Execute the following commands to prepare the server
            //netsh http add urlacl url=https://+:port/ user=Everyone
            //netsh http add sslcert ipport=0.0.0.0:port certhash=certThumbprint appid={50e6f7c6-6ca0-4477-b3c7-178141c0df60}
            //Note: The self test will fail if the ssl certificate isn't valid - This is the expected behaviour and should NOT be changed

            var config = _host.StartsWith("https") ? new SecureHttpSelfHostConfiguration($"{_host}:{_port}") : new HttpSelfHostConfiguration($"{_host}:{_port}");

            config.Routes.MapHttpRoute("AnimeApi", "1.0/{controller}/", new { });
            config.Routes.MapHttpRoute("AnimeItemApi", "1.0/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("SeasonData", "1.0/{controller}/{season}/year/{year}", new { controller = "Season", season = RouteParameter.Optional, year = RouteParameter.Optional });
            config.Routes.MapHttpRoute("Health", "1.0/{controller}/", new { });
            return config;
        }

        /// <summary>
        /// Setup Logging
        /// </summary>
        private static void SetupLogging()
        {
            var logToConsole = ConfigurationKeys.LogToConsole.GetConfigurationValue<bool>();
            var logToFile = ConfigurationKeys.LogToFile.GetConfigurationValue<bool>();
            var logToSplunk = ConfigurationKeys.LogToSplunk.GetConfigurationValue<bool>();
            var logFile = ConfigurationKeys.LogPath.GetConfigurationValue<string>();

            var configuration = new LoggerConfiguration();
            if (logToConsole)
            {
                configuration.WriteTo.ColoredConsole();
            }
            if (logToFile)
            {
                configuration.WriteTo.File(logFile);
            }
            if (logToSplunk)
            {
                var splunkUri = ConfigurationKeys.SplunkUrl.GetConfigurationValue<string>();
                var splunkToken = ConfigurationKeys.SplunkToken.GetConfigurationValue<string>();
                configuration.WriteTo.SplunkViaEventCollector(splunkUri, splunkToken);
            }
            Log.Logger = configuration.CreateLogger();
        }

        private static void ReadSettings()
        {
            _host = ConfigurationManager.AppSettings["Host"];
            _port = ConfigurationManager.AppSettings["Port"];
        }

        private static string GetGitInformation()
        {
            var attr = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false) as AssemblyInformationalVersionAttribute[];
            if (attr == null)
            {
                return Assembly.GetEntryAssembly()?.GetName().Version.ToString() ?? EmptyVersion;
            }
            var value = attr[0].InformationalVersion.Split(' ');
            return $"{value[0]} ({value[2].Split('=')[1]})";
        }
    }
}
