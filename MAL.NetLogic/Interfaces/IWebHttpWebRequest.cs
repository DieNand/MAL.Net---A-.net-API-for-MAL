using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface IWebHttpWebRequest
    {
        CookieContainer CookieContainer { get; set; }
        string UserAgent { get; set; }
        string Method { get; set; }
        string ContentType { get; set; }
        HttpStatusCode StatusCode { get; }

        void CreateRequest(string url);
        Stream GetRequestStream();
        void GetResponse();
        Stream GetResponseStream();
        void SetCredentials(string username, string password);
    }
}