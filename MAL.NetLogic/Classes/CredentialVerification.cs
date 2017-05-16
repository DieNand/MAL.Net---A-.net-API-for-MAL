using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Classes
{
    public class CredentialVerification : ICredentialVerification
    {
        #region Variables

        private const string Url = "https://myanimelist.net/api/account/verify_credentials.xml";

        #endregion

        #region Public Methods

        public async Task<bool> VerifyCredentials(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.Default.GetBytes($"{username}:{password}"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                var response = await httpClient.GetAsync(Url);
                return response.IsSuccessStatusCode;
            }
        }

        #endregion

    }
}