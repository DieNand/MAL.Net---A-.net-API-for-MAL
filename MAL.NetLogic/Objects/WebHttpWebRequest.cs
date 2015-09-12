using System.IO;
using System.Net;
using System.Net.Mime;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class WebHttpWebRequest : IWebHttpWebRequest
    {
        #region Variables

        private WebRequest _webRequest;
        private HttpWebRequest _httpRequest;
        private HttpWebResponse _response;

        #endregion

        #region Properties

        public CookieContainer CookieContainer
        {
            get { return _httpRequest.CookieContainer; }
            set { _httpRequest.CookieContainer = value; }
        }

        public string UserAgent
        {
            get { return _httpRequest.UserAgent; }
            set { _httpRequest.UserAgent = value; }
        }

        public string Method
        {
            get { return _httpRequest.Method; }
            set { _httpRequest.Method = value; }
        }

        public string ContentType
        {
            get { return _httpRequest.ContentType; }
            set { _httpRequest.ContentType = value; }
        }

        public HttpStatusCode StatusCode => _response.StatusCode;

        #endregion
        #region Public Methods

        public void CreateRequest(string url)
        {
            _webRequest = WebRequest.Create(url);
            _httpRequest = (HttpWebRequest) _webRequest;
            _httpRequest.AutomaticDecompression = DecompressionMethods.GZip;
        }

        public Stream GetRequestStream()
        {
            return _webRequest.GetRequestStream();
        }

        public void GetResponse()
        {
            _response = (HttpWebResponse)_httpRequest.GetResponse();
        }

        public Stream GetResponseStream()
        {
            return _response.GetResponseStream();
        }

        #endregion
    }
}