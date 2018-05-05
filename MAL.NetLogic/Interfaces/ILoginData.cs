using System.Net;

namespace MAL.NetLogic.Interfaces
{
    public interface ILoginData
    {
        string Username { get; set; }
        string Password { get; set; }
        CookieContainer Cookies { get; set; }
        bool LoginValid { get; set; }
        bool CanCache { get; set; }
    }
}