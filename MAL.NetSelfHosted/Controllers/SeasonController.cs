using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace MAL.NetSelfHosted.Controllers
{
    /// <summary>
    /// Controller to retrieve season data
    /// </summary>
    public class SeasonController : ApiController
    {
        #region Variables

        private readonly ISeasonRetriever _seasonRetriever;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="seasonRetriever"></param>
        public SeasonController(ISeasonRetriever seasonRetriever)
        {
            _seasonRetriever = seasonRetriever;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all shows for a spesific season
        /// </summary>
        /// <param name="year">The year for which the data should be retrieved</param>
        /// <param name="season">The season for which the data should be retrieved</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get(string season, int year)
        {
            var watch = new Stopwatch();
            watch.Start();
            Log.Information("Received request for {year} - {season}", year, season);
            var seasonData = await _seasonRetriever.GetSeasonData(year, season);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var stringResponse = JsonConvert.SerializeObject(seasonData);
            response.Content = new StringContent(stringResponse, Encoding.UTF8, "application/json");
            watch.Stop();
            Log.Information("Sent response for season request {year} - {season}. Processing took {duration}", year, season, watch.Elapsed);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get()
        {
            var watch = new Stopwatch();
            watch.Start();
            Log.Information("Received request for current season data");
            var data = await _seasonRetriever.RetrieveCurrentSeason();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var stringResponse = JsonConvert.SerializeObject(data);
            response.Content = new StringContent(stringResponse, Encoding.UTF8, "application/json");
            watch.Stop();
            Log.Information("Send current season data. Processing took {duration}", watch.Elapsed);
            return response;
        }

        #endregion
    }
}