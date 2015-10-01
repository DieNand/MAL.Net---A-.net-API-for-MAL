using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Classes
{
    public class DataPush : IDataPush
    {
        #region Variables

        private const string AddShowUrl = "http://myanimelist.net/api/animelist/add/{0}.xml";
        private const string EditShowUrl = "http://myanimelist.net/api/animelist/update/{0}.xml";
        private readonly IConsoleWriter _consoleWriter;
        private readonly IWebHttpWebRequestFactory _webHttpWebRequestFactory;
        private readonly IMappingToJson _jsonMapper;
        private readonly IAnimeListRetriever _animeListRetriever;
        private readonly string _userAgent;

        #endregion

        #region Constructor

        public DataPush(IConsoleWriter consoleWriter, IWebHttpWebRequestFactory webHttpWebRequestFactory, IMappingToJson jsonMapper, IAnimeListRetriever animeListRetriever)
        {
            _userAgent = ConfigurationManager.AppSettings["UserAgent"];
            _consoleWriter = consoleWriter;
            _webHttpWebRequestFactory = webHttpWebRequestFactory;
            _jsonMapper = jsonMapper;
            _animeListRetriever = animeListRetriever;
        }

        #endregion

        #region Public Methods

        public async Task<bool> PushAnimeDetailsToMal(IAnimeDetailsJson details, string username, string password, bool canCache = true)
        {
            var useDetails = _jsonMapper.ConvertJsonAnimeDetailsToAnimeDetails(details);

            var result = false;
            Console.WriteLine($"{DateTime.Now} - [DataPush] Received request to update {details.AnimeId} for {username}");
            var userlist = await _animeListRetriever.GetAnimeList(username);
            var item = userlist.Anime.FirstOrDefault(t => t.SeriesId == details.AnimeId);
            //The item doesn't exists - Use the add new method
            if (item == null)
            {
                result = await UpdateAnimeDetails(useDetails, username, password, canCache);
                Console.WriteLine($"{DateTime.Now} - [DataPush] Added {details.AnimeId} for {username}");
            }
            else
            {
                result = await UpdateAnimeDetails(useDetails, username, password, canCache, true);
                if (result)
                    Console.WriteLine($"{DateTime.Now} - [DataPush] Updated {details.AnimeId} for {username}");
                else
                {
                    Console.Write($"{DateTime.Now} - ");
                    _consoleWriter.WriteAsLineEnd($"[DataPush] Update of {details.AnimeId} failed for {username}", ConsoleColor.Red);
                }
            }
            return result;
        }

        #endregion

        #region Private Methods

        private async Task<bool> UpdateAnimeDetails(IAnimeDetails details, string username, string password, bool canCache = true, bool isupdate = false)
        {
            try
            {
                var updateRequest = _webHttpWebRequestFactory.Create();
                updateRequest.CreateRequest(isupdate
                    ? string.Format(EditShowUrl, details.AnimeId)
                    : string.Format(AddShowUrl, details.AnimeId));
                updateRequest.UserAgent = _userAgent;
                updateRequest.SetCredentials(username, password);
                updateRequest.Method = WebRequestMethods.Http.Post;
                updateRequest.ContentType = "application/x-www-form-urlencoded";
                var requestStream = updateRequest.GetRequestStream();
                var requestWriter = new StreamWriter(requestStream);
                requestWriter.Write($"data={_jsonMapper.ConvertAnimeDetailsToXml(details)}");
                requestWriter.Close();
                var response = GetResponse(updateRequest);
                if (response.Contains("Error"))
                {
                    Console.Write($"{DateTime.Now} - ");
                    _consoleWriter.WriteAsLineEnd($"[DataPush] Failed to update the anime.\r\nError: {response}", ConsoleColor.Red);
                    return false;
                }
                Console.WriteLine($"{DateTime.Now} - [DataPush] Successfully completed the anime update request");
                return true;

            }
            catch (Exception ex)
            {
                Console.Write($"{DateTime.Now} - ");
                _consoleWriter.WriteAsLineEnd($"[DataPush] Error occured while updating anime.\r\n{ex}", ConsoleColor.Red);
                return false;
            }
        }

        private string GetResponse(IWebHttpWebRequest request)
        {
            var result = string.Empty;
            try
            {
                //var response = await request.GetResponseAsync();
                request.GetResponse();
                var statusCode = request.StatusCode;
                if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created)
                {
                    using (var stream = request.GetResponseStream())
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
                    Console.WriteLine($"{DateTime.Now} - [DataPush] - Got response {statusCode} from server");
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} - ");
                _consoleWriter.WriteAsLineEnd($"[DataPush] Error occured while waiting for web response. {ex}", ConsoleColor.Red);
                throw;
            }
            return result;
        }
        
        #endregion
    }
}
 