using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MAL.NetLogic.Interfaces;

namespace MAL.NetSelfHosted.Controllers
{
    public class AuthenticationController : ApiController
    {
        #region Variables

        private readonly ICredentialVerification _credentialVerification;
        private readonly Stopwatch _stopwatch;

        #endregion

        #region Constructor

        public AuthenticationController(ICredentialVerification credentialVerification)
        {
            _credentialVerification = credentialVerification;
            _stopwatch = new Stopwatch();
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
            _stopwatch.Reset();
            _stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - [Auth] Received credential verification request for {username}");
            var result = await _credentialVerification.VerifyCredentials(username, password);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent($"Valid Credetials: {result}");
            _stopwatch.Start();
            Console.WriteLine($"{DateTime.Now} - [Auth] Verification completed for {username}. Processing took {_stopwatch.Elapsed}");

            return response;
        }
        
        #endregion
    }
}