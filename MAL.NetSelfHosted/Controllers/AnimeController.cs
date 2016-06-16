using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using MAL.NetSelfHosted.Interfaces;
using Serilog;

namespace MAL.NetSelfHosted.Controllers
{
    /// <summary>
    /// Controller used to retrieve anime details
    /// </summary>
    public class AnimeController : ApiController
    {
        #region Variables

        private readonly IAnimeHandler _animeHandler;
        private readonly IAnimeListRetriever _animeListRetriever;
        private readonly IMappingToJson _mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="animeHandler"></param>
        /// <param name="animeListRetriever"></param>
        /// <param name="mapper"></param>
        public AnimeController(IAnimeHandler animeHandler, IAnimeListRetriever animeListRetriever, IMappingToJson mapper)
        {
            _animeHandler = animeHandler;
            _animeListRetriever = animeListRetriever;
            _mapper = mapper;
        }

        #endregion

        /// <summary>
        /// Get method used by the API to test availability.
        /// </summary>
        /// <returns>Returns a constant string</returns>
        public string Get()
        {
            Log.Information("Naked request received");
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
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Information("Reveived request for {Anime Id}", id);
            string anime;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                anime = await _animeHandler.HandleRequest(id);
            }
            else
            {
                anime = await _animeHandler.HandleRequest(id, username, password);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(anime, Encoding.UTF8, "application/json");
            stopWatch.Stop();
            Log.Information("Sent response for {Anime Id}. Processing took {Duration}", id, stopWatch.Elapsed);

            return response;
        }
    }

}