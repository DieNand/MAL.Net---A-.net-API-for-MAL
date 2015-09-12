using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface IUserAuthentication
    {
        ILoginData Login(string username, string password, bool canCache = true);
    }
}