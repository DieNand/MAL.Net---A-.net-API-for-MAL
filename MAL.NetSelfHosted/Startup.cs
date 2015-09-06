using System.Net;
using System.Web.Http;
using AutoMapper;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Factories;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using MAL.NetSelfHosted.Handlers;
using MAL.NetSelfHosted.Interfaces;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;

[assembly: OwinStartup(typeof(MAL.NetSelfHosted.Startup))]

namespace MAL.NetSelfHosted
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "1.0/{controller}/{id}", new { id = RouteParameter.Optional });
            app.UseWebApi(config);


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

            app.Use(async (context, next) =>
            {
                using (container.BeginExecutionContextScope())
                {
                    await next();
                }
            });



        }
    }
}
