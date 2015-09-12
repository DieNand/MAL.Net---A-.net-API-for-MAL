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
    public class MyListController : ApiController
    {
        #region Variables

        private readonly IAnimeHandler _animeHandler;
        private readonly IAnimeListRetriever _animeListRetriever;
        private readonly IMappingToJson _mapper;
        private readonly Stopwatch _stopwatch;

        #endregion

        #region Constructor

        public MyListController(IAnimeHandler animeHandler, IAnimeListRetriever animeListRetriever, IMappingToJson mapper)
        {
            _animeHandler = animeHandler;
            _animeListRetriever = animeListRetriever;
            _mapper = mapper;
            _stopwatch = new Stopwatch();
        }

        #endregion

        [Route("1.0/MyList/{username}")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string username)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            Console.Write($"{DateTime.Now} - [Anime] Received request for {username}'s anime list");
            var userlist = await _animeListRetriever.GetAnimeList(username);
            var jsonList = _mapper.ConvertMyListToJson(userlist);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonList, Encoding.UTF8, "application/json");
            _stopwatch.Stop();
            Console.WriteLine($"{DateTime.Now} - [Anime] Sent response for {username}'s anime list. Processing took {_stopwatch.Elapsed}");

            return response;
        }
    }
}