using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface IUserAuthentication
    {
        Task<ILoginData> Login(string username, string password, bool canCache = true);
    }
}