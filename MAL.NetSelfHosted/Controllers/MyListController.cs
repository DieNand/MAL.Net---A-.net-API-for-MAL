using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using Serilog;

namespace MAL.NetSelfHosted.Controllers
{
    /// <summary>
    /// Controller to retrieve user list
    /// </summary>
    public class MyListController : ApiController
    {
        #region Variables

        private readonly IAnimeListRetriever _animeListRetriever;
        private readonly IMappingToJson _mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="animeListRetriever"></param>
        /// <param name="mapper"></param>
        public MyListController(IAnimeListRetriever animeListRetriever, IMappingToJson mapper)
        {
            _animeListRetriever = animeListRetriever;
            _mapper = mapper;
        }

        #endregion

        /// <summary>
        /// Retrieve user list for a spesific user
        /// </summary>
        /// <param name="username">MAL Username</param>
        /// <returns>List containing User information and all anime in the user's list</returns>
        public async Task<HttpResponseMessage> Get(string username)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Information("Received request for {username}'s anime list", username);
            var userlist = await _animeListRetriever.GetAnimeList(username);
            var jsonList = _mapper.ConvertMyListToJson(userlist);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonList, Encoding.UTF8, "application/json");
            stopWatch.Stop();
            Log.Information("Sent response for {username}'s list. Processing took {duration}", username, stopWatch.Elapsed);

            return response;
        }
    }
}