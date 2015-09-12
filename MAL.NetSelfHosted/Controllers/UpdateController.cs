using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;

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

        [Route("1.0/Update/{username, password, cancache}")]
        public async Task<HttpResponseMessage> Post(string username, string password, bool cancache, [FromBody] IAnimeDetails updateDetails)
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