using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using MAL.NetSelfHosted.Interfaces;

namespace MAL.NetSelfHosted.Controllers
{
    public class AnimeController : ApiController
    {
        #region Variables

        private readonly IAnimeHandler _animeHandler;
        private readonly IAnimeListRetriever _animeListRetriever;
        private readonly IMappingToJson _mapper;
        private readonly Stopwatch _stopwatch;

        #endregion

        #region Constructor

        public AnimeController(IAnimeHandler animeHandler, IAnimeListRetriever animeListRetriever, IMappingToJson mapper)
        {
            _animeHandler = animeHandler;
            _animeListRetriever = animeListRetriever;
            _mapper = mapper;
            _stopwatch = new Stopwatch();
        }

        #endregion

        public string Get()
        {
            Console.WriteLine("Naked request received");
            return "Call with an ID to get an anime value";
        }

        public async Task<HttpResponseMessage> Get(int id)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - [Anime] Received request for Anime ID {id}");
            var anime = await _animeHandler.HandleRequest(id);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(anime, Encoding.UTF8, "application/json");
            _stopwatch.Stop();
            Console.WriteLine($"{DateTime.Now} - [Anime] Sent response for Anime ID {id}. Processing took {_stopwatch.Elapsed}");

            return response;
        }

        public async Task<HttpResponseMessage> Get(int id, string username, string password)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - [Anime] Received request for Anime ID {id}");
            var anime = await _animeHandler.HandleRequest(id, username, password);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(anime, Encoding.UTF8, "application/json");
            _stopwatch.Stop();
            Console.WriteLine($"{DateTime.Now} - [Anime] Sent response for Anime ID {id}. Processing took {_stopwatch.Elapsed}");

            return response;
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}