using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using Serilog;

namespace MAL.NetSelfHosted.Controllers
{
    /// <summary>
    /// Constoller used to update user list
    /// </summary>
    public class UpdateController : ApiController
    {
        #region Variables

        private readonly IDataPush _dataPush;

        #endregion

        #region Constructor

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="dataPush"></param>
        public UpdateController(IDataPush dataPush)
        {
            _dataPush = dataPush;
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
        public async Task<HttpResponseMessage> Post(string username, string password, bool cancache, [FromBody] AnimeDetailsJson updateDetails)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Information("Received update request for {username} for {Anime Id}", username, updateDetails.AnimeId);
            var result = await _dataPush.PushAnimeDetailsToMal(updateDetails, username, password, cancache);
            var response = Request.CreateResponse(result ? HttpStatusCode.OK : HttpStatusCode.NotModified);
            Log.Information("Successfully completed update of {AnimeId} for {username}: {Update Result}. Processing took {duration}", updateDetails.AnimeId, username, result, stopWatch.Elapsed);
            return response;
        }
        
        #endregion


    }
}