using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;
using Serilog;

namespace MAL.NetSelfHosted.Controllers
{
    /// <summary>
    /// Controller used to retrieve Authentication details
    /// </summary>
    public class AuthenticationController : ApiController
    {
        #region Variables

        private readonly ICredentialVerification _credentialVerification;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentialVerification"></param>
        public AuthenticationController(ICredentialVerification credentialVerification)
        {
            _credentialVerification = credentialVerification;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verify if the user credentials are valid
        /// </summary>
        /// <param name="username">MAL Username</param>
        /// <param name="password">MAL Password</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get([FromUri] string username, [FromUri] string password)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Information("Received credential verification request for {username}", username);
            bool result;
            try
            {
                result = await _credentialVerification.VerifyCredentials(username, password);
            }
            catch (UnauthorizedAccessException)
            {
                Log.Information("Received unauthorized - Credentials for {username} isn't valid", username);
                result = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occured while trying to validate user credentails");
                result = false;
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent($"Valid Credetials: {result}");
            stopWatch.Start();
            Log.Information("Verification completed for {username}. Processing took {duration}", username, stopWatch.Elapsed);

            return response;
        }
        
        #endregion
    }
}