using System.Net;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class LoginData : ILoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public CookieContainer Cookies { get; set; }
        public bool LoginValid { get; set; }
        public bool CanCache { get; set; } 
    }
}