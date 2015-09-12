using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;

namespace MAL.NetSelfHosted.Controllers
{
    public class UpdateController : ApiController
    {
        #region Variables

        private readonly IDataPush _dataPush;
        private readonly Stopwatch _stopwatch;

        #endregion

        #region Constructor

        public UpdateController(IDataPush dataPush)
        {
            _dataPush = dataPush;
            _stopwatch = new Stopwatch();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update an anime in a user's anime list. If the anime doesn't exist in the user's list it will be added with the details provided.
        /// </summary>
        /// <param name="username">MAL Username</param>
        /// <param name="password">MAL Password</param>
        /// <param name="cancache">Can the server cache the user's login temporarily</param>
        /// <param name="updateDetails">The details for the show to be updated</param>
        /// <returns>OK - Updated succeeded, NotModified - Error occured, no update happened</returns>
        public async Task<HttpResponseMessage> Post(string username, string password, bool cancache, [FromBody] AnimeDetails updateDetails)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - [Update] Received update request from {username} for {updateDetails.AnimeId}");
            var result = await _dataPush.PushAnimeDetailsToMal(updateDetails, username, password, cancache);
            var response = Request.CreateResponse(result ? HttpStatusCode.OK : HttpStatusCode.NotModified);
            Console.WriteLine($"{DateTime.Now} - [Update] Successfully completed update of {updateDetails.AnimeId} for {username}: {result}");
            return response;
        }
        
        #endregion


    }
}