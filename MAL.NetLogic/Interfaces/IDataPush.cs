using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface IDataPush
    {
        Task<bool> PushAnimeDetailsToMal(IAnimeDetails details, string username, string password, bool canCache = true);
    }
}