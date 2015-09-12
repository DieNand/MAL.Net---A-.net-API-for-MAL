using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using HtmlAgilityPack;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Classes
{
    public class AnimeListRetriever : IAnimeListRetriever
    {
        #region Variables

        private readonly string _userAgent;
        private const string AnimeListUrl = "http://myanimelist.net/malappinfo.php?u={0}&status=all&type=anime";
        private readonly IWebHttpWebRequestFactory _webHttpWebRequestFactory;
        private readonly IAnimeFactory _animeFactory;
        private readonly IMappingToJson _mapper;
        private readonly IConsoleWriter _consoleWriter;


        #endregion

        #region Constructor

        public AnimeListRetriever(IWebHttpWebRequestFactory webHttpWebRequestFactory, IAnimeFactory animeFactory, IMappingToJson mapper, IConsoleWriter consoleWriter)
        {
            _webHttpWebRequestFactory = webHttpWebRequestFactory;
            _userAgent = ConfigurationManager.AppSettings["UserAgent"];
            _animeFactory = animeFactory;
            _mapper = mapper;
            _consoleWriter = consoleWriter;
        }

        #endregion

        #region Public Methods

        public async Task<IMyAnimeList> GetAnimeList(string username)
        {
            var cookies = new CookieContainer();

            var request = _webHttpWebRequestFactory.Create();
            var listUrl = string.Format(AnimeListUrl, username);
            request.CreateRequest(listUrl);
            request.UserAgent = _userAgent;
            request.Method = WebRequestMethods.Http.Get;
            GetCsrfToken(cookies);
            request.CookieContainer = cookies;

        
            //Use getResponse to get the response
            var result = await GetResponse(request);
            var myList = ConvertXml(result);

            return myList;
        }

        /// <summary>
        /// To be able to do a login we need both the CSRF token embedded in the webpage as well as the cookies.
        /// Retrieve the required values by visiting the login page
        /// </summary>
        /// <returns>CSRF Token embedded in Login Page</returns>
        private void GetCsrfToken(CookieContainer cookieContainer)
        {
            var doc = new HtmlDocument();
            var loginRequest = _webHttpWebRequestFactory.Create();
            loginRequest.CreateRequest(@"http://myanimelist.net/login.php");
            loginRequest.CookieContainer = cookieContainer;
            loginRequest.GetResponse();

        }

        #endregion

        #region Private Methods

        private IMyAnimeList ConvertXml(string xmlString)
        {
            var mylist = _animeFactory.CreateAnimeList();
            try
            {
                var xml = XDocument.Parse(xmlString);
                var userInfo = xml.Root?.Element("myinfo");
                var userAnime = xml.Root?.Elements("anime").ToList();

                if(userInfo == null || userAnime.Count == 0)
                    throw new Exception("Failed to retrieve or parse User's My Anime List");

                var xmlInfoSerializer = new XmlSerializer(typeof(MyInfoXml));
                var info = (MyInfoXml)xmlInfoSerializer.Deserialize(userInfo.CreateReader());
                mylist.Info = _mapper.ConvertMyInfoFromXmlToObject(info);

                var xmlAnimeSerializer = new XmlSerializer(typeof(ListAnimeXml));
                foreach (var item in userAnime)
                {
                    var anime = (ListAnimeXml)xmlAnimeSerializer.Deserialize(item.CreateReader());
                    mylist.Anime.Add(_mapper.ConvertListAnimeXmlToObject(anime));
                }
            }
            catch (Exception ex)
            {
                Console.Write($"{DateTime.Now} - ");
                _consoleWriter.WriteAsLineEnd($"[ListRetriever] Error occured fetching list.\r\n{ex}", ConsoleColor.Red);
            }
            return mylist;
        }

        private async Task<string> GetResponse(IWebHttpWebRequest request)
        {
            var result = string.Empty;
            try
            {
                //var response = await request.GetResponseAsync();
                request.GetResponse();
                var statusCode = request.StatusCode;
                if (statusCode == HttpStatusCode.OK)
                {
                    using (var stream = request.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            //var buffer = new byte[2048];
                            using (var desinationStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(desinationStream);
                                //stream.CopyTo(desinationStream);

                                result = Encoding.UTF8.GetString(desinationStream.ToArray());
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - [ListRetriever] - Got response {statusCode} from server");
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} - ");
                _consoleWriter.WriteAsLineEnd($"[ListRetriever] Error occured while waiting for web response. {ex}", ConsoleColor.Red);
            }
            return result;
        }

        #endregion
    }
}