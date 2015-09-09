using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Classes
{
    public class UserAuthentication : IUserAuthentication
    {
        #region Variables

        private const string LoginUrl = @"http://myanimelist.net/login.php";
        private readonly string _userAgent;
        private readonly IAuthFactory _authFactory;

        #endregion

        #region Constructor

        public UserAuthentication(IAuthFactory authFactory)
        {
            _userAgent = ConfigurationManager.AppSettings["UserAgent"];
            _authFactory = authFactory;
        }

        #endregion

        #region Public Methods

        public async Task<ILoginData> Login(string username, string password, bool canCache = true)
        {
            var loginData = _authFactory.CreateLingData();

            loginData.Username = username;
            loginData.Password = password;
            loginData.CanCache = canCache;

            var cookieJar = new CookieContainer();

            var loginRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);
            loginRequest.CookieContainer = cookieJar;
            loginRequest.UserAgent = _userAgent;
            loginRequest.Method = WebRequestMethods.Http.Post;
            loginRequest.ContentType = "application/x-www-form-urlencoded";

            var csrfToken = GetCsrfToken(cookieJar);
            var requestText = $"user_name={username}&password={password}&cookie=1&sublogin=Login&submit=1&csrf_token={csrfToken}";
            var stream = loginRequest.GetRequestStream();
            var requestWriter = new StreamWriter(stream);
            requestWriter.Write(requestText);
            requestWriter.Close();

            var response = await GetResponse(loginRequest);

            if (!string.IsNullOrEmpty(response))
            {
                if (response.Contains("Could not find that username") || response.Contains("Password is incorrect"))
                {
                    Console.WriteLine($"{DateTime.Now} - [Auth] Auth failed for username and password pair");
                    loginData.LoginValid = false;
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - [Auth] Auth succeeded for username and password pair");
                    loginData.LoginValid = true;
                    loginData.Cookies = cookieJar;

                }
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} - [Auth] No response received from the server");
                loginData.LoginValid = false;
            }
            return loginData;

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To be able to do a login we need both the CSRF token embedded in the webpage as well as the cookies.
        /// Retrieve the required values by visiting the login page
        /// </summary>
        /// <returns>CSRF Token embedded in Login Page</returns>
        private string GetCsrfToken(CookieContainer cookieContainer)
        {
            var doc = new HtmlDocument();
            var loginRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);
            loginRequest.CookieContainer = cookieContainer;

            doc.Load(loginRequest.GetResponse().GetResponseStream());
            var csrfToken = doc.DocumentNode.SelectSingleNode("//meta[@name='csrf_token']").Attributes["content"].Value;

            return csrfToken;
        }

        private async Task<string> GetResponse(HttpWebRequest request)
        {
            var result = string.Empty;
            try
            {
                //var response = await request.GetResponseAsync();
                var response = request.GetResponse();
                var statusCode = ((HttpWebResponse)response).StatusCode;
                if (statusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            var buffer = new byte[2048];
                            using (var desinationStream = new MemoryStream())
                            {
                                int bytesRead;
                                do
                                {
                                    bytesRead = stream.Read(buffer, 0, 2048);
                                    desinationStream.Write(buffer, 0, bytesRead);
                                } while (bytesRead != 0);

                                result = Encoding.UTF8.GetString(desinationStream.ToArray());

                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - [Auth] - Got response {statusCode} from server");
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} - [Auth] Error occured while waitin for web response. {ex}");
            }
            return result;
        }

        #endregion
    }
}