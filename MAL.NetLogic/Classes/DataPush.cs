using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;
using Serilog;

namespace MAL.NetLogic.Classes
{
    public class DataPush : IDataPush
    {
        #region Variables

        private const string AddShowUrl = "http://myanimelist.net/api/animelist/add/{0}.xml";
        private const string EditShowUrl = "http://myanimelist.net/api/animelist/update/{0}.xml";
        private readonly IWebHttpWebRequestFactory _webHttpWebRequestFactory;
        private readonly IMappingToJson _jsonMapper;
        private readonly IAnimeListRetriever _animeListRetriever;
        private readonly string _userAgent;

        #endregion

        #region Constructor

        public DataPush(IWebHttpWebRequestFactory webHttpWebRequestFactory, IMappingToJson jsonMapper, IAnimeListRetriever animeListRetriever)
        {
            _userAgent = ConfigurationManager.AppSettings["UserAgent"];
            _webHttpWebRequestFactory = webHttpWebRequestFactory;
            _jsonMapper = jsonMapper;
            _animeListRetriever = animeListRetriever;
        }

        #endregion

        #region Public Methods

        public async Task<bool> PushAnimeDetailsToMal(IAnimeDetailsJson details, string username, string password, bool canCache = true)
        {
            var useDetails = _jsonMapper.ConvertJsonAnimeDetailsToAnimeDetails(details);

            bool result;
            Log.Information("Received request to update {AnimeId} for {username}", details.AnimeId, username);
            var userlist = await _animeListRetriever.GetAnimeList(username);
            var item = userlist.Anime.FirstOrDefault(t => t.SeriesId == details.AnimeId);
            //The item doesn't exists - Use the add new method
            if (item == null)
            {
                result = await UpdateAnimeDetails(useDetails, username, password, canCache);
                Log.Information("Added {AnimeId} for {username}", details.AnimeId, username);
            }
            else
            {
                result = await UpdateAnimeDetails(useDetails, username, password, canCache, true);
                if (result)
                {
                    Log.Information("Updated {AnimeId} for {username}", details.AnimeId, username);
                }
                else
                {
                    Log.Warning("Update of {AnimeId} for {username} failed", details.AnimeId, username);
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
                    Log.Warning("Failed to update {AnimeId} for {username}. Response received was {Response}", details.AnimeId, username, response);
                    return false;
                }
                Log.Information("Successfully completed the update of {AnimeId} for {username}", details.AnimeId, username);
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured while trying to update {AnimeId} for {username}", details.AnimeId, username);
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
                    Log.Warning("Received {StatusCode} from server", statusCode);
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured while waiting for web response");
                throw;
            }
            return result;
        }
        
        #endregion
    }
}
 