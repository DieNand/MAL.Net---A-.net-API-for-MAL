using System.Web.Http;

namespace MAL.NetSelfHosted.Controllers
{
    /// <summary>
    /// Health Controller
    /// </summary>
    public class HealthController : ApiController
    {
        #region Public Methods

        /// <summary>
        /// Get health status
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return "MAL.Net is online";
        }

        #endregion
    }



}