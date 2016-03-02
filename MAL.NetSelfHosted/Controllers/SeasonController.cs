using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetSelfHosted.Controllers
{
    public class SeasonController : ApiController
    {
        #region Variables

        private readonly ISeasonRetriever _seasonRetriever;

        #endregion

        #region Constructor

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
        public async Task<HttpResponseMessage> Get(int year, string season)
        {
            var watch = new Stopwatch();
            watch.Start();
            Console.WriteLine($"{DateTime.Now} - [Season] Received request for {year} - {season}");
            var seasonData = await _seasonRetriever.GetSeasonData(year, season);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var stringResponse = JsonConvert.SerializeObject(seasonData);
            response.Content = new StringContent(stringResponse, Encoding.UTF8, "application/json");
            watch.Stop();
            Console.WriteLine($"{DateTime.Now} - [Season] Sent response for season request {year} - {season}");
            return response;
        }

        #endregion
    }
}