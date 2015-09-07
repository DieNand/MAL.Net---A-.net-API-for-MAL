using System.Net;
using System.Threading.Tasks;

namespace MAL.NetLogic.Classes
{
    public class CredentialVerification
    {
        #region Variables

        private const string Url = "http://myanimelist.net/api/account/verify_credentials.xml";

        #endregion

        #region Public Methods

        public async Task<bool> VerifyCredentials(string username, string password)
        {
            var request = WebRequest.Create(Url);
            request.Method = "GET";
            request.UseDefaultCredentials = false;
            request.Credentials = new NetworkCredential(username, password);
            var response = await request.GetResponseAsync();
            return ((HttpWebResponse) response).StatusCode == HttpStatusCode.OK;
        }

        #endregion

    }
}