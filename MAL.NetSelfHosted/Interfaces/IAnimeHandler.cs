using System.Threading.Tasks;

namespace MAL.NetSelfHosted.Interfaces
{
    public interface IAnimeHandler
    {
        Task<string> HandleRequest(int id);
        Task<string> HandleRequest(int id, string username, string password);
    }
}