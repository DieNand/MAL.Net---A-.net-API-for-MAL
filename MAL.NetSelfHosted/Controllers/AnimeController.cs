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

        /// <summary>
        /// Get method used by the API to test availability.
        /// </summary>
        /// <returns>Returns a constant string</returns>
        public string Get()
        {
            Console.WriteLine("Naked request received");
            return "Call with an ID to get an anime value";
        }

        /// <summary>
        /// Get method that returns the details for an Anime from MAL.
        /// If a username and password is provided the user's current watch status and episode will be returned
        /// </summary>
        /// <param name="id">MAL Anime ID</param>
        /// <param name="username">MAL Username</param>
        /// <param name="password">MAL Password</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get(int id, [FromUri] string username = "", [FromUri] string password = "")
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - [Anime] Received request for Anime ID {id}");
            string anime = string.Empty;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                anime = await _animeHandler.HandleRequest(id);
            else
                anime = await _animeHandler.HandleRequest(id, username, password);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(anime, Encoding.UTF8, "application/json");
            _stopwatch.Stop();
            Console.WriteLine($"{DateTime.Now} - [Anime] Sent response for Anime ID {id}. Processing took {_stopwatch.Elapsed}");

            return response;
        }
    }

    public class AuthDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}