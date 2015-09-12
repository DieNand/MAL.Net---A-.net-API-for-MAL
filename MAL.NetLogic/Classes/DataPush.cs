using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Classes
{
    public class DataPush
    {
        #region Variables

        private const string AddShowUrl = "http://myanimelist.net/panel.php?go=add&selected_series_id={0}";
        private const string EditShowUrl = "http://myanimelist.net/editlist.php?type=anime&id={0}&hideLayout";
        private readonly IConsoleWriter _consoleWriter;
        private readonly ICacheHandler _cacheHandler;
        private readonly IWebHttpWebRequestFactory _webHttpWebRequestFactory;
        private readonly IMappingToJson _jsonMapper;
        private readonly string _userAgent;

        #endregion

        #region Constructor

        public DataPush(IConsoleWriter consoleWriter, ICacheHandler cacheHandler, IWebHttpWebRequestFactory webHttpWebRequestFactory, IMappingToJson jsonMapper)
        {
            _userAgent = ConfigurationManager.AppSettings["UserAgent"];
            _consoleWriter = consoleWriter;
            _cacheHandler = cacheHandler;
            _webHttpWebRequestFactory = webHttpWebRequestFactory;
            _jsonMapper = jsonMapper;
        }

        #endregion

        #region Public Methods

        public void PushAnimeDetailsToMal(IAnimeDetails details, string username, string password, bool canCache = true)
        {
            
        }

        /*
        try
            {
                if (Helpers.NetworkHelper.isInternetOnline)
                {
                    //The item is in MAL - do an update
                    if (CookieJar.getInstance.getMALList.Where(t => t.series_animedb_id == watchDetails.animeID).Count() > 0)
                    {
                        Logger.getInstance.addInformation(moduleName, "Running update for " + watchDetails.animeID);
                        doUpdate(CookieJar.getInstance.loginCookies, watchDetails);
                    }
                    //The item is new - do an add
                    else
                    {
                        Logger.getInstance.addInformation(moduleName, "Adding new item for " + watchDetails.animeID);
                        addNew(CookieJar.getInstance.loginCookies, watchDetails);
                    }
                }
                else
                {
                    Logger.getInstance.addWarning(moduleName, "Cannot sync, internet is offline");
                    //Failed sync needs to be stored for later sync
                    LiveSync.getInstance.addForFailedSync(watchDetails);
                }
            }
            catch(Exception ex)
            {
                Logger.getInstance.addWarning(moduleName, "Failed to sync with MAL\r\n" + ex.ToString());
                LiveSync.getInstance.addForFailedSync(watchDetails);
            }*/

        #endregion

        #region Private Methods

        private async Task<bool> UpdateAnimeDetails(IAnimeDetails details, string username, string password, bool canCache = true)
        {
            try
            {
                var loginData = await _cacheHandler.GetAuth(username, password, canCache);
                if (!loginData.LoginValid)
                {
                    Console.Write($"{DateTime.Now} - ");
                    _consoleWriter.WriteAsLineEnd($"[DataPush] Login Failed - Cannot update details", ConsoleColor.Red);
                    return false;
                }
                var updateRequest = _webHttpWebRequestFactory.Create();
                updateRequest.CreateRequest(string.Format(EditShowUrl, details.AnimeId));
                updateRequest.UserAgent = _userAgent;
                updateRequest.CookieContainer = loginData.Cookies;
                updateRequest.Method = WebRequestMethods.Http.Post;
                updateRequest.ContentType = "application/x-www-form-urlencoded";
                var requestStream = updateRequest.GetRequestStream();
                var requestWriter = new StreamWriter(requestStream);
                requestWriter.Write(_jsonMapper.ConvertAnimeDetailsToXml(details));
                requestWriter.Close();
                var response = GetResponse(updateRequest);
                if (response == "Error")
                {
                    Console.Write($"{DateTime.Now} - ");
                    _consoleWriter.WriteAsLineEnd($"[DataPush] Failed to update the anime", ConsoleColor.Red);
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
                if (statusCode == HttpStatusCode.OK)
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
                _consoleWriter.WriteAsLineEnd("[DataPush] Error occured while waiting for web response. {ex}", ConsoleColor.Red);
            }
            return result;
        }
        
        #endregion
    }
}
 